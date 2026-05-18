const AppHelper = {
    getCookie(name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) return parts.pop().split(';').shift();
        return null;
    },

    showToast(message, type = 'info') {
        const normalizedType = type === 'error' ? 'error' : type === 'success' ? 'success' : type === 'warning' ? 'warning' : 'info';

        if (window.Swal) {
            Swal.fire({
                toast: true,
                position: 'top-end',
                icon: normalizedType,
                title: message,
                showConfirmButton: true,
                timer: 3000,
                timerProgressBar: true
            });
        } else if (window.toastr) {
            toastr.options = {
                closeButton: true,
                progressBar: true,
                timeOut: 3000
            };
            toastr[type](message);
        } else {
            console.log(type.toUpperCase() + ": " + message);
            alert(message);
        }
    },

    showSuccessAndRedirect(message, redirectUrl, delay = 1800) {
        if (window.Swal) {
            Swal.fire({
                icon: 'success',
                title: 'Shipment Created',
                text: message,
                timer: delay,
                timerProgressBar: true,
                showConfirmButton: true,
                allowOutsideClick: false
            }).then(() => {
                window.location.href = redirectUrl;
            });
        } else {
            this.showToast(message, 'success');
            setTimeout(() => {
                window.location.href = redirectUrl;
            }, delay);
        }
    },

    formatDate(date) {
        if (!date) return '';
        return new Date(date).toLocaleDateString();
    },

    formatDateTime(date) {
        if (!date) return '';
        return new Date(date).toLocaleString();
    },

    getQueryParam(name) {
        const urlParams = new URLSearchParams(window.location.search);
        return urlParams.get(name);
    },

    getIdFromPath() {
        const segments = window.location.pathname.split('/').filter(Boolean);
        return segments.length ? segments[segments.length - 1] : null;
    },
        
    parseJwt(token) {
        try {
            return JSON.parse(atob(token.split('.')[1]));
        } catch {
            return null;
        }
    },

    isTokenExpired(token) {
        const decoded = this.parseJwt(token);
        if (!decoded || !decoded.exp) return true;

        const now = Math.floor(Date.now() / 1000);
        return decoded.exp < now;
    }
};
