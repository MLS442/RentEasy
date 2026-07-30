import { createContext, useState } from "react";

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [token, setToken] = useState(() => {
        return localStorage.getItem("token") || ''
    })
    
    const [user, setUser] = useState(() => {
        return JSON.parse(localStorage.getItem("user")) || null
    })

    const login = (receivedToken, userObject) => {
        setToken(receivedToken)
        setUser(userObject)

        localStorage.setItem("token", receivedToken)
        localStorage.setItem("user", JSON.stringify(userObject))
    }

    const logout = () => {
        setToken(null)
        setUser(null)

        localStorage.removeItem("token")
        localStorage.removeItem("user")
    }

    return (
        <>
            <AuthContext.Provider value={{token, user, login, logout}}>
                {children}
            </AuthContext.Provider>
        </>
    )
}