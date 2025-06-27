<template>
<div>
      <div v-if="!jwtToken">
        <Message v-if="failedLogin" severity="error" v-text="errMsg"></Message>
        <br>
        <InputText name="username" type="text" placeholder="Username" v-model="addName"/><br><br>
        <InputText name="password" type="password" placeholder="Password" v-model="addPass"/><br><br>
        <Button type="submit" severity="secondary" label="Register" @click="Register"/><br><br>
        <Button type="submit" severity="secondary" label="Login" @click="Login"/>
      </div>
</div>
</template>
<script setup lang="ts">
import { defineProps, onMounted, watch, ref  } from "vue";
import useApi, { useApiRawRequest,apiUrl } from "@/models/api";
import { useRouter} from 'vue-router'
import Message from 'primevue/message';
import InputText from 'primevue/inputtext';
import Button from 'primevue/button';
import { computed } from 'vue'
const addName = ref('')
const addPass = ref('')
let failedLogin = ref(false);
let errMsg = ref('')
const router = useRouter()
const jwtToken = computed(() => !!localStorage.getItem("jwt"))
if(localStorage.getItem("jwt")){
    router.push("/")
}
function Register(){
  const resp = fetch(apiUrl+"Auth/register"+"?username="+addName.value + "&password=" + addPass.value,{
      method: 'POST',
      headers: {
        Accept: 'application/json',
        'Content-Type': 'application/json',
      }}).then((resp)=>{
        if(resp.status === 200){
          return resp.json()
        }
        else if (resp.status === 400){
          errMsg.value = "That username already exists!"
          failedLogin.value = true
        }
      }).catch((error) => {
        errMsg.value = "Failed to reach backend!"
        failedLogin.value = true
    }).then((jason =>{
          localStorage.setItem("jwt",jason.token)
          localStorage.setItem("t",jason.type)
          localStorage.setItem("username",jason.name)
          router.go()
    }));
}
function Login(){
    router.push("/")
}
</script>
