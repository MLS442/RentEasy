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
  const [tickets, setTickets] = useState([])
  const [tenants, setTenants] = useState([])
  const [properties, setProperties] = useState([])
  const [isLoading, setIsLoading] = useState(true)

  async function handleFixTicket(ticketToUpdate) {
    const url = `https://localhost:7130/Tickets/${ticketToUpdate.ticketId}`
    const updatedTicket = { ...ticketToUpdate, status: "Fixed" }
    delete updatedTicket.tenant
    try {
      const response = await fetch(url, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(updatedTicket)
      })
      if (!response.ok) throw new Error(`Response Status: ${response.status}`)
      setTickets(tickets.map(t => t.ticketId === ticketToUpdate.ticketId ? { ...t, status: "Fixed" } : t))
    } catch { console.error('failed') }
  }

  async function deleteTicket(id) {
    const url = `https://localhost:7130/Tickets/${id}`
    try {
      const response = await fetch(url, { method: "DELETE" })
      if (!response.ok) throw new Error(`Response Status: ${response.status}`)
      setTickets(tickets.filter(t => t.ticketId != id))
    } catch { console.error('failed') }
  }

  useEffect(() => {
    async function getMockTickets() {
      const url = "https://localhost:7130/Tickets"
      try {
        const response = await fetch(url)
        if (!response.ok) throw new Error(`Response Status: ${response.status}`)
        const result = await response.json()
        setTickets(result)
      } catch { console.error('failed') } finally { setIsLoading(false) }
    }
    getMockTickets()

    async function getMockTenants() {
      const url = "https://localhost:7130/Tenants"
      try {
        const response = await fetch(url)
        if (!response.ok) throw new Error(`Response Status: ${response.status}`)
        const result = await response.json()
        setTenants(result)
      } catch { console.error('failed') } finally { setIsLoading(false) }
    }
    getMockTenants()

    async function getMockProperties() {
      const url = "https://localhost:7130/Properties"
      try {
        const response = await fetch(url)
        if (!response.ok) throw new Error(`Response Status: ${response.status}`)
        const result = await response.json()
        setProperties(result)
      } catch { console.error('failed') } finally { setIsLoading(false) }
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
              tickets={tickets}
              setTickets={setTickets}
              isLoading={isLoading} 
            />
          } />

          <Route path="landlord-dashboard" element={
            <LandlordDashboard 
              name={landLord}
              tickets={tickets}
              tenants={tenants}
              properties={properties}
              handleFixTicket={handleFixTicket}
              isLoading={isLoading}
              deleteTicket={deleteTicket} 
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
