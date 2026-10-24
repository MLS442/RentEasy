export async function Register(userInfo) {
    const url = "https://localhost:7130/Auth/Register"

    const response = await fetch(url, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(userInfo)
    })

    return response
}

export async function Login(userInfo) {
    const url = "https://localhost:7130/Auth/Login"

    const response = await fetch(url, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify(userInfo)
    })

    return response
}

const AuthService = {Register, Login};
export default AuthService;