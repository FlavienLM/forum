import { mount } from '@vue/test-utils';
import SignUpPage from '@/pages/SignUpPage.vue'; // Adjust the path if necessary
import userService from '@/api/services/userService';
import sessionService from '@/api/services/sessionService';
import { describe, it, expect, vi } from 'vitest';

// Mock the services
vi.mock('@/api/services/userService');
vi.mock('@/api/services/sessionService');

describe('SignUpPage.vue', () => {

    it('renders the signup form', () => {
        const wrapper = mount(SignUpPage);

        expect(wrapper.find('h1').text()).toBe("S'inscrire");
        expect(wrapper.find('input#accountId').exists()).toBe(true);
        expect(wrapper.find('input#pseudo').exists()).toBe(true);
        expect(wrapper.find('input#email').exists()).toBe(true);
        expect(wrapper.find('input#password').exists()).toBe(true);
        expect(wrapper.find('button[type="submit"]').text()).toBe("S'inscrire");
    });

    it('shows an error message when trying to register with a taken accountId', async () => {
        // Mock the userService.getUserList to return a list with a taken accountId
        vi.mocked(userService.getUserList).mockResolvedValue([{
            accountId: 'existingUser',
            pseudo: '',
            mail: ''
        }]);

        const wrapper = mount(SignUpPage);

        // Wait for the fetchUsers onMounted call to finish
        await wrapper.vm.$nextTick();

        // Set form input values
        await wrapper.find('input#accountId').setValue('existingUser');

        // Trigger form submission
        await wrapper.find('form').trigger('submit.prevent');

        // Wait for the async action
        await wrapper.vm.$nextTick();

        // Check the error message
        expect(wrapper.find('.text-red-500').text()).toBe("L'identifiant est déjà pris. Veuillez en choisir un autre.");
    });

    it('calls userService.createUser and sessionService.setItem on successful signup', async () => {
        // Mock userService to return an empty user list (no conflicts)
        vi.mocked(userService.getUserList).mockResolvedValue([]);

        // Mock the userService response to return a new user ID
        vi.mocked(userService.createUser).mockResolvedValueOnce(1);

        const wrapper = mount(SignUpPage);

        // Wait for the fetchUsers onMounted call to finish
        await wrapper.vm.$nextTick();

        // Set form input values
        await wrapper.find('input#accountId').setValue('newUser');
        await wrapper.find('input#pseudo').setValue('NewPseudo');
        await wrapper.find('input#email').setValue('newemail@example.com');
        await wrapper.find('input#password').setValue('password123');

        // Trigger form submission
        await wrapper.find('form').trigger('submit.prevent');

        // Wait for the async action
        await wrapper.vm.$nextTick();

        // Verify userService.createUser was called with correct arguments
        expect(userService.createUser).toHaveBeenCalledWith({
            pseudo: 'NewPseudo',
            password: 'password123',
            mail: 'newemail@example.com',
            accountId: 'newUser',
            date: expect.any(String), // Should match any formatted date string
        });

        // Verify sessionService.setItem was called with correct user data
        expect(sessionService.setItem).toHaveBeenCalledWith({
            id: 1,
            pseudo: 'NewPseudo',
            mail: 'newemail@example.com',
            accountId: 'newUser',
        });
    });
});
