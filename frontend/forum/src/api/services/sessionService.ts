import type { User } from "@/types/types";
import { ref } from 'vue';
const key = 'user_session';
const isLoggedIn = ref(localStorage.getItem(key) != undefined);

const sessionService = {
    setItem(value: User) {
      try {
        localStorage.setItem(key, JSON.stringify(value));
        isLoggedIn.value = true;
      } catch (error) {
        console.error("Error saving to localStorage", error);
      }
    },
  
    getItem(): User|null {
      try {
        const value = localStorage.getItem(key);
        return value ? JSON.parse(value) : null;
      } catch (error) {
        console.error("Error reading from localStorage", error);
        return null;
      }
    },
    
    logout() {
      try {
        localStorage.clear();
        isLoggedIn.value=false;
      } catch (error) {
        console.error("Error clearing localStorage", error);
      }
    },

    logged() {
      return isLoggedIn.value;
    },

    getIdUser() {
      return this.getItem()?.id
    }

  };
  
export default sessionService;