import { useState, useEffect } from 'react'
import TicketService from '../services/TicketService'

export const LandlordDashboard = ({ name, tenants, properties }) => {

    const [tickets, setTickets] = useState([])
    const [isLoading, setIsLoading] = useState(true)
    // Note: In the conversation you also declared tenants and properties state here:
    // const [tenants, setTenants] = useState([])
    // const [properties, setProperties] = useState([])

    useEffect(() => {
        async function getTickets() {
            try {
                const response = await TicketService.getTickets()
                if (!response.ok) throw new Error(`Response Status: ${response.status}`)
                const result = await response.json()
                setTickets(result)
            } catch (error) {
                console.error('Failed to fecth the tickets:', error)
                alert("Could not load the tickets. Please try again.")
            }
            finally{
                setIsLoading(false)
            }
        }
        getTickets()
    }, []);

    async function handleFixTicket(ticketToUpdate) {
        const updatedTicket = { ...ticketToUpdate, status: "Fixed" }

        try {
            const response = await TicketService.FixTicket(updatedTicket)

            if (!response.ok) throw new Error(`Response Status: ${response.status}`)
            setTickets(prevTickets => prevTickets.map(t =>
                t.ticketId === ticketToUpdate.ticketId ? { ...t, status: "Fixed" } : t))
        } catch (error) {
            console.error("Failed to fix ticket:", error)
            alert("Could not update the ticket. Please try again.")
        }
    }

    async function deleteTicket(id) {
        try {
            const response = await TicketService.deleteTicket(id)
            if (!response.ok) throw new Error(`Response Status: ${response.status}`)
            setTickets(prevTickets => prevTickets.filter(t => t.ticketId !== id))
        } catch (error) {
            console.error("Failed to delete the ticket:", error)
            alert("Could not delete the ticket. Please try again.")
        }
    }

    if (isLoading) {
        return (
            <div>
                <p>Loading...</p>
            </div>
        )
    }

    return (
        <>
           <h2>Hello {name} </h2>

            <div>
              <h2>Tickets List</h2>
            </div>

           <ul>
               {tickets.map(t => (<li key= {t.ticketId}>
                {t.ticketId},
                {t.tenantFullName},
                {t.subject}, 
                {t.description},
                {t.status},   <button onClick={() => handleFixTicket(t)}>Mark as fixed</button> <button onClick={() => deleteTicket(t.ticketId)}> Delete</button>
               </li>))} 
           </ul> <br />

           <div>
              <h2>Tenants List</h2>
           </div>

           <ul>
              {tenants.map(t => (<li key= {t.tenantId}>
                {t.tenantId},
                {t.fullName},
                {t.address},
                {t.email},
                {t.phone},
                {t.birthDate}
              </li>))}
           </ul>

           <div>
              <h2>Properties List</h2>
           </div>

           <ul>
              {properties.map(p => (<li key= {p.propertyId}>
                {p.propertyId},
                {p.address},
                {p.price},
                {p.bedrooms},
                {p.leaseEndDate}
              </li>))}
           </ul>
        </>
    )
}