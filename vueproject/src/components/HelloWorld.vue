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
    import { setLanguage } from '../utils/cookieManager'

    export default defineComponent({
        data() {
            return {
                loading: false,
                post: null,
                locale:'zh',
            };
        },
        created() {
            // fetch the data when the view is created and the data is
            // already being observed
            this.axiosData();
        },
        watch: {
            // call again the method if the route changes
            '$route': 'axiosData',
            locale: function (val) {
                setLanguage(val)
                this.$i18n.locale = val
                this.axiosData()
            }
        },
        methods: {
            fetchData() {
                this.post = null;
                this.loading = true;

                fetch('weatherforecast')
                    .then(r => r.json())
                    .then(json => {
                        this.post = json.data;
                        this.loading = false;
                        return;
                    });
            },
            axiosData() {
                const timer = setTimeout(() => {
                    this.post = null;
                    this.loading = true;
                }, 500)

                this.axios.get('weatherforecast').then(data => {
                    this.post = data;
                    clearTimeout(timer);
                    this.loading = false;
                })
            },
            switchLocale(newLocale) {
                this.locale = newLocale
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