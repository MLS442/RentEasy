import { useState, useContext } from 'react'
import { useNavigate } from 'react-router-dom';
import { AuthContext } from '../contexts/AuthContext';
import AuthService from '../services/AuthService'

export default function Login() {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [isLoading, setIsLoading] = useState(false)
  const [error, setError] = useState('')

  const navigate = useNavigate();

  const { login } = useContext(AuthContext);

  async function handleLogin() {
    const userCredentials = { email: email, password: password }


    try {
      const response = await AuthService.Login(userCredentials)
      //if (!response.ok) throw new Error(`Response Status: ${response.status}`)

      if (!response.ok) {
        const text = await response.text()
        setError(text)

        return
      }

      const result = await response.json()

      login(result.accessToken, result.user)

      if (result.user.role === "Landlord") {
        navigate('/landlord-dashboard')
      }
      else if (result.user.role === 'Tenant') {
        navigate('/tenant-dashboard');
      }
      else {
        throw new Error(`Unsupported user role: ${result.user.role}`);
      }


    }
    catch (err) {
      console.error("Failed to log in:", err)
      setError("Unable to connect to the server. Please try again later.")
    }
  }

  // Separate handler function
  async function handleSubmit(e) {
    e.preventDefault(); // Prevents the default browser page reload

    setError("");

    if (email === "" || password === "") {
      setError("Please fill out all fields.");
      return;
    }

    if (password.length < 6) {
      setError("Password must be at least 6 characters long.");
      return;
    }

    setIsLoading(true);

    await handleLogin();

    setIsLoading(false);
  }

  return (
    <>
      <h1>Login Page</h1>

      {error ? <p>{error}</p> : null}

      <form onSubmit={handleSubmit}>

        <input
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          placeholder='email'
        />
        <br />

        <input
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          placeholder='password'
        />
        <br />
        <br />

        {/* 4. The submit button that automatically triggers the form's onSubmit */}
        <button type="submit" disabled={isLoading}>{isLoading ? "Logging in..." : "Login"}</button>

      </form>
      <br />
    </>
  )
}
