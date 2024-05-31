<template>
    <div class="weather-component">
        <h1>{{$t('Weather forecast')}}</h1>
        <p>This component demonstrates fetching data from the server.</p>

        <div v-if="loading" class="loading">
            Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationvue">https://aka.ms/jspsintegrationvue</a> for more details.
        </div>
        <div v-if="post" class="content">
            <table>
                <thead>
                    <tr>
                        <th>{{$t('Date')}}</th>
                        <th>{{$t('Temp.')}} (C)</th>
                        <th>{{$t('Temp.')}} (F)</th>
                        <th>{{$t('Summary')}}</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="forecast in post" :key="forecast.date">
                        <td>{{ forecast.date }}</td>
                        <td>{{ forecast.temperatureC }}</td>
                        <td>{{ forecast.temperatureF }}</td>
                        <td>{{ forecast.summary }}</td>
                    </tr>
                </tbody>
            </table>
        </div>
    </div>
    <div>
        <button @click="switchLocale('en')">English</button>
        <button @click="switchLocale('zh')">中文</button>
    </div>
</template>

<script lang="js">
    import { defineComponent } from 'vue';
    import { setLanguage } from '../config/cookie'
    import userInfo from '../config/stores/userInfo'

    export default defineComponent({
        data() {
            return {
                loading: false,
                post: null,
                locale: 'zh',
                user : userInfo(),
            };
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.updateUser();
            this.axiosData();
        },
        mounted(){
            console.log(`id:${this.user.id}`);
        },
        watch: {
            // call again the method if the route changes
            '$route': 'axiosData',
            locale: function (val) {
                setLanguage(val);
                this.$i18n.locale = val;
                this.axiosData();
            }
        },
        computed: {

        },
        methods: {
            axiosData() {
                const timer = setTimeout(() => {
                    this.post = null;
                    this.loading = true;
                }, 500);

                this.axios.get('weatherforecast').then(data => {
                    this.post = data;
                    clearTimeout(timer);
                    this.loading = false;
                });
            },
            switchLocale(newLocale) {
                this.locale = newLocale;
            },
            updateUser(){
                this.user.updateUser({ id: '08d3fa5a-9ae6-ee11-9c29-5a44875600c1', name: 'test' })
            }
        },
    });
</script>

<style scoped>
    th {
        font-weight: bold;
    }

    tr:nth-child(even) {
        background: #F2F2F2;
    }

    tr:nth-child(odd) {
        background: #FFF;
    }

    th, td {
        padding-left: .5rem;
        padding-right: .5rem;
    }

    .weather-component {
        text-align: center;
    }

    table {
        margin-left: auto;
        margin-right: auto;
    }
</style>