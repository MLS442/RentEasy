import { useState } from 'react'

export default function Login() {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [isLoading, setIsLoading] = useState(false)

  async function handleLogin() {

  }

  // Separate handler function
  async function handleSubmit(e) {
    e.preventDefault(); // Prevents the default browser page reload

    setIsLoading(true);
    setError("");

    if (email === "" || password === "") {
      setError("Please fill out all fields.");
      return;
    }

    if (password.length < 6) {
      setError("Password must be at least 6 characters long.");
      return;
    }

    await handleLogin();

    setIsLoading(false);
  }

  return (
    <>
      <h1>Login Page</h1>

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
