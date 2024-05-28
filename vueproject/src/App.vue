<script setup>
import HelloWorld from './components/HelloWorld.vue'
import TheWelcome from './components/TheWelcome.vue'
</script>

<template>
    <header>
        <!--<img alt="Vue logo" class="logo" src="./assets/logo.svg" width="125" height="125" />-->

        <div class="wrapper">
            <!--<HelloWorld />-->
            <router-view></router-view>
        </div>
        <router-link to="/home">Home</router-link>
        <button><router-link to="/">HelloWorld</router-link></button>
        <router-link to="/about">About</router-link>
    </header>

  <main>
    <TheWelcome />
  </main>
</template>

<style scoped>
header {
  line-height: 1.5;
}

.logo {
  display: block;
  margin: 0 auto 2rem;
}

@media (min-width: 1024px) {
  header {
    display: flex;
    place-items: center;
    padding-right: calc(var(--section-gap) / 2);
  }

  .logo {
    margin: 0 2rem 0 0;
  }

  header .wrapper {
    display: flex;
    place-items: flex-start;
    flex-wrap: wrap;
  }
}
</style>
