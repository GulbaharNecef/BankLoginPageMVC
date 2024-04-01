// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
sessionStorage.setItem('accessToken', response.data.accessToken);
//axios.defaults.headers.common['Authorization'] = 'Bearer ' + localStorage.getItem('accessToken');

fetch('/Privacy', {
    headers: { // Include in all requests to authorized endpoints
        'Authorization': 'Bearer ' + sessionStorage.getItem('accessToken')
    }
})