export function getLanguage() {
    const cookie = document.cookie;
    if (cookie === '.AspNetCore.Culture=c=en-US|uic=en-US') {
        return 'en';
    } else {
        return 'zh';
    }
}

export function setLanguage(newLocale) {
    if (newLocale === 'en') {
        document.cookie = '.AspNetCore.Culture=c=en-US|uic=en-US';
    } else {
        document.cookie = '.AspNetCore.Culture=c=zh-CN|uic=zh-CN';
    }
}