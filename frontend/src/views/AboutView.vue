<template>  
  <EventList :title="title"/> 
</template>

<script setup lang="ts">
  import { useRouter} from 'vue-router'
  import { computed } from 'vue'
  const router = useRouter()
  if(!localStorage.getItem("jwt")){
      router.push("/")
  }

  import {defineProps} from "vue";
  
  defineProps<{ title: String}>();
</script>
