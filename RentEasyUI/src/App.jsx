import './App.css'
import { TenantDashboard } from './components/TenantDashboard'
import { LandlordDashboard } from './components/LandlordDashboard'
import { useState, useEffect } from 'react'

//const mockData = [{ticket_id: 1, tenant_id: 1, subject: "My room", description: "There is a mouse in it", status: "Not Fixed"}]


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
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(updatedTicket)
      })

      if (!response.ok) {
        throw new Error(`Response Status: ${response.status}`)
      }


      setTickets(tickets.map(t => {
        if (t.ticketId === ticketToUpdate.ticketId) {
          const fixedTicket = { ...t, status: "Fixed" }
          return fixedTicket
        }
        else {
          return t
        }
      }))
    }
    catch {
      console.error('failed')
    }
  }

  async function deleteTicket(id) {
    const url = `https://localhost:7130/Tickets/${id}`

    try {
      const response = await fetch(url, {
        method: "DELETE"
      })

      if (!response.ok) {
        throw new Error(`Response Status: ${response.status}`)
      }

      setTickets(tickets.filter(t => {
        return t.ticketId != id
      }))

    }
    catch {
      console.error('failed')
    }
  }

  /*Fecthing mockData: This method block the user from typing until data loads (The "Loading..." spienner)
  useEffect(() => {
      const timer = setTimeout(() => {
          setTickets([mockData[0]])
          setIsLoading(false)
      }, 3000)

      return () => {
      clearTimeout(timer)
     }
  },[]);*/




  useEffect(() => {
    async function getMockTickets() {
      const url = "https://localhost:7130/Tickets"
      try {
        const response = await fetch(url)
        if (!response.ok) {
          throw new Error(`Response Status: ${response.status}`)
        }

        const result = await response.json()
        //console.log(result)



        setTickets(result)
      }
      catch {
        console.error('failed')
      }
      finally {
        setIsLoading(false)
      }
    }
    getMockTickets()

    async function getMockTenants() {
      const url = "https://localhost:7130/Tenants"
      try {
        const response = await fetch(url)
        if (!response.ok) {
          throw new Error(`Response Status: ${response.status}`)
        }

        const result = await response.json()
        //console.log(result)

        setTenants(result)
      }
      catch {
        console.error('failed')
      }
      finally {
        setIsLoading(false)
      }
    }
    getMockTenants()

    async function getMockProperties() {
      const url = "https://localhost:7130/Properties"
      try {
        const response = await fetch(url)
        if (!response.ok) {
          throw new Error(`Response Status: ${response.status}`)
        }

        const result = await response.json()
        //console.log(result)

        setProperties(result)
      }
      catch {
        console.error('failed')
      }
      finally {
        setIsLoading(false)
      }
    }
    getMockProperties()

  }, []);

  return (
    <>
      <h1>RentEasy</h1>
      <TenantDashboard name={tenant}
        tickets={tickets}
        setTickets={setTickets}
        isLoading={isLoading} />

      <LandlordDashboard name={landLord}
        tickets={tickets}
        tenants={tenants}
        properties={properties}
        handleFixTicket={handleFixTicket}
        isLoading={isLoading}
        deleteTicket={deleteTicket} />
    </>
  )
}

export default App 
