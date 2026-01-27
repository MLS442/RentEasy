import './App.css'
import { TenantDashboard } from './components/TenantDashboard'
import { LandlordDashboard } from './components/LandlordDashboard'
import { useState } from 'react'

function App() {
  const tenant = "Alex"
  const landLord = "Mohamed"
  const [tickets,setTickets] = useState([])

 
  
    const handleFixTickets = (id) => {

       setTickets(tickets.map(t => {
        if (t.ticket_id === id){
          const fixedTicket = {...t, status:"Fixed"}
          return fixedTicket
        }
        else {
          return t
        }
      }))

      

    }

  return (
    <>
      <h1>RentEasy</h1>
      <TenantDashboard name = {tenant}
      tickets={tickets} 
      setTickets={setTickets}/>

      <LandlordDashboard name= {landLord} tickets={tickets} fixTicket={handleFixTickets}/>
    </>
  ) 
}

export default App 
