<script setup lang="ts">
import { RouterView } from 'vue-router';
import HeaderLayout from './components/layouts/HeaderLayout.vue';
import SideBarLayout from './components/layouts/SideBarLayout.vue';
import FooterLayout from './components/layouts/FooterLayout.vue';

</script>

<template>

<div class="app-layout">
    <!-- Header -->
    <HeaderLayout />

    <div class="content-wrapper">
      <!-- Sidebar -->
      <SideBarLayout />

      <!-- Main Content (Dynamically rendered page) -->
      <main class="main-content pt-[5rem] bg-gray-700">
        <RouterView/>

      </main>
    </div>

    <!-- Footer -->
    <FooterLayout />
  </div>

  <main>

  </main>
</template>

<style scoped>
.app-layout {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
  margin: 0;
}

.content-wrapper {
  display: flex;
  flex-grow: 1;
}

.main-content {
  flex-grow: 1;
  background-color: #f9f9f9;
}
</style>