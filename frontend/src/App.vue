<script setup lang="ts">
import { RouterLink, RouterView } from 'vue-router'
import { computed } from 'vue'

const login = computed(() => !!localStorage.getItem("jwt"))
</script>

<template>
  <nav>
    <RouterLink to="/">Home</RouterLink>
    <div v-if="login">
        <RouterLink to="/notes">Notes</RouterLink><br>
        <RouterLink to="/create">People</RouterLink><br>
    </div>
  </nav>
  <RouterView />  
</template>

<style scoped>
</style>
