import './App.css'
import { TenantDashboard } from './components/TenantDashboard'
import { LandlordDashboard } from './components/LandlordDashboard'
import Login from './components/Login'
import Register from './components/Register'
import { useState, useEffect } from 'react'
import { Routes, Route, Link } from 'react-router-dom'

function App() {
  const tenant = "Alex"
  const landLord = "Mohamed"
  const [tenants, setTenants] = useState([])
  const [properties, setProperties] = useState([])


  useEffect(() => {
    async function getMockTenants() {
      const url = "https://localhost:7130/Tenants"
      try {
        const response = await fetch(url)
        if (!response.ok) throw new Error(`Response Status: ${response.status}`)
        const result = await response.json()
        setTenants(result)
      } catch {
        console.error('failed')
      }
    }
    getMockTenants()
  }, []);

  useEffect(() => {
    async function getMockProperties() {
      const url = "https://localhost:7130/Properties"
      try {
        const response = await fetch(url)
        if (!response.ok) throw new Error(`Response Status: ${response.status}`)
        const result = await response.json()
        setProperties(result)
      } catch {
        console.error('failed')
      }
    }
    getMockProperties()
  }, []);


  return (
    <>
      <header className="main-header">
        <h1>RentEasy</h1>

        <nav className="nav-menu">
          <Link to="/login">Login</Link>
          <Link to="/register/sample-token-123">Register</Link>
          <Link to="/tenant-dashboard">Tenant Dashboard</Link>
          <Link to="/landlord-dashboard">Landlord Dashboard</Link>
        </nav>

      </header>

      <main className="content-container">
        <Routes>

          <Route path="login" element={<Login />} />

          <Route path="register/:token" element={<Register />} />

          <Route path="tenant-dashboard" element={
            <TenantDashboard
              name={tenant}
            />
          } />

          <Route path="landlord-dashboard" element={
            <LandlordDashboard
              name={landLord}
              tenants={tenants}
              properties={properties}
            />
          } />

          <Route path="*" element={
            <div className="error-page">
              <h2>404 - Page Not Found</h2>
            </div>
          } />

        </Routes>
      </main>
    </>
  )
}


export default App
