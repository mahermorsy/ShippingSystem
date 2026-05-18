const ApiClient = {
    baseUrl: (window.appConfig && window.appConfig.apiBaseUrl) ? window.appConfig.apiBaseUrl : 'https://localhost:7007',

    getAccessToken: function () {
        return AppHelper.getCookie("AccessToken");
    },

    buildHeaders: function (useAuth = true) {
        const headers = {
            'Content-Type': 'application/json'
        };

        const accessToken = this.getAccessToken();

        if (useAuth && accessToken) {
            headers['Authorization'] = 'Bearer ' + accessToken;
        }

        return headers;
    },

    request: function (method, url, data, onSuccess, onError, useAuth = true, retry = true) {
        $.ajax({
            url: this.baseUrl + url,
            type: method,
            contentType: 'application/json',
            data: data ? JSON.stringify(data) : null,
            headers: this.buildHeaders(useAuth),
            xhrFields: {
                withCredentials: true
            },
            success: onSuccess,
            error: (xhr) => {
                if (useAuth && xhr.status === 401 && retry) {
                    this.refreshToken(
                        () => this.request(method, url, data, onSuccess, onError, useAuth, false),
                        onError
                    );
                } else if (onError) {
                    onError(xhr);
                }
            }
        });
    },

    get: function (url, onSuccess, onError, useAuth = true) {
        this.request('GET', url, null, onSuccess, onError, useAuth);
    },

    post: function (url, data, onSuccess, onError, useAuth = true) {
        this.request('POST', url, data, onSuccess, onError, useAuth);
    },

    put: function (url, data, onSuccess, onError, useAuth = true) {
        this.request('PUT', url, data, onSuccess, onError, useAuth);
    },

    delete: function (url, onSuccess, onError, useAuth = true) {
        this.request('DELETE', url, null, onSuccess, onError, useAuth);
    },

    refreshToken: function (onSuccess, onFailure) {

        const refreshToken = AppHelper.getCookie("RefreshToken");

        if (!refreshToken) {
            if (onFailure) onFailure({ message: "No refresh token" });
            window.location.href = "/Account/Login";
            return;
        }

        $.ajax({
            url: this.baseUrl + '/api/auth/refresh-token',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                refreshToken: refreshToken
            }),
            success: function (response) {
                if (response && response.accessToken) {

                    document.cookie = `AccessToken=${response.accessToken}; path=/; secure`;
                    document.cookie = `RefreshToken=${response.refreshToken}; path=/; secure`;

                    if (onSuccess) onSuccess();
                } else {
                    if (onFailure) onFailure({ message: 'Token refresh failed.' });
                }
            },
            error: function (err) {
                window.location.href = "/Account/Login";
                if (onFailure) onFailure(err);
            }
        });
    }
};
