// tests/LoginPage.spec.ts
import { mount } from '@vue/test-utils';
import { describe, it, expect, vi } from 'vitest';
import LoginPage from '@/pages/LoginPage.vue';
import sessionService from '@/api/services/sessionService';
import userService from '@/api/services/userService';
import type { User } from '@/types/types';

vi.mock('@/api/services/sessionService'); // Mock session service
vi.mock('@/api/services/userService'); // Mock user service

describe('LoginPage.vue', () => {
  it('renders the login form', () => {
    const wrapper = mount(LoginPage);

    expect(wrapper.find('h1').text()).toBe('Se connecter');
    expect(wrapper.find('input#pseudo').exists()).toBe(true);
    expect(wrapper.find('input#password').exists()).toBe(true);
    expect(wrapper.find('button[type="submit"]').text()).toBe('Se connecter');
  });

  it('shows an error when login fails', async () => {
    // Mock the userService.getUserList to return an empty list
    vi.mocked(userService.getUserList).mockResolvedValueOnce([]);

    const wrapper = mount(LoginPage);
    await wrapper.find('input#pseudo').setValue('wrongUser');
    await wrapper.find('input#password').setValue('wrongPass');
    await wrapper.find('form').trigger('submit.prevent');

    // Wait for the async action
    await wrapper.vm.$nextTick();

    expect(wrapper.find('.text-red-500').text()).toBe('Mauvais mot de passe ou pseudo');
  });

  it('calls sessionService.setItem on successful login', async () => {
    const mockUser: User = {
      id: 1,
      accountId: 'correctUser',
      password: 'correctPass',
      pseudo: 'TestUser',
      mail: 'test@test.com'
    };

    // Mock the userService.getUserList to return the correct user
    vi.mocked(userService.getUserList).mockResolvedValueOnce([mockUser]);

    const wrapper = mount(LoginPage);
    await wrapper.find('input#pseudo').setValue('correctUser');
    await wrapper.find('input#password').setValue('correctPass');
    await wrapper.find('form').trigger('submit.prevent');

    // Wait for the async action
    await wrapper.vm.$nextTick();

    // Check that sessionService.setItem was called with correct user data
    expect(sessionService.setItem).toHaveBeenCalledWith({
      pseudo: 'TestUser',
      mail: 'test@test.com',
      accountId: 'correctUser',
      id: 1,
      date: ''
    });
  });
});
