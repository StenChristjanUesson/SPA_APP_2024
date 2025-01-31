<template>  
  <EventList :title="title"/> 
</template>

<script setup lang="ts">
  import EventList from "@/components/EventList.vue"
  import {defineProps} from "vue";
  
  defineProps<{ title: String}>();
</script>
