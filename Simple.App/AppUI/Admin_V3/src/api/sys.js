import http from '@/utils/http.js'

export function login(body) {
    return http.post('/api/auth/login', body)
}
export function userinfo() {
    return http.get('/api/auth/userinfo')
}

export function usermenus() {
    return http.get('/api/auth/usermenus')
}

export function qRole(op,param) {
    return http.get('/api/Role/'+ op, param)
}
export function dalRole(op,param,method = 'post') {
    return http.request({
        url: '/api/Role/'+ op,
        method,
        data: param
    })
}

export function qUser(op,param) {
    return http.get('/api/User/'+ op, param)
}
export function dalUser(op,param,method = 'post') {
    return http.request({
        url: '/api/User/'+ op,
        method,
        data: param
    })
}

export function qMenu(op,param) {
    return http.get('/api/Menu/'+ op, param)
}
export function dalMenu(op,param,method = 'post') {
    return http.request({
        url: '/api/Menu/'+ op,
        method,
        data: param
    })
}