import { useState, useEffect } from 'react'
import TicketService from '../services/TicketService'

export const TenantDashboard = ({ name }) => {

    const [tickets, setTickets] = useState([])
    const [isLoading, setIsLoading] = useState(true)
    const [subject, setSubject] = useState('')
    const [description, setDescription] = useState('')

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

    async function createTicket() {
        if (subject === '' || description === '') {
            return alert("The fields should not be empty")
        }

        const newTicket = {
            //tenantId: 1,
            subject: subject,
            description: description,
            status: "Not Fixed"
        }

        try {
            const response = await TicketService.postTicket(newTicket)

            if (!response.ok) throw new Error(`Response Status: ${response.status}`)

            const savedTicket = await response.json()

            setTickets([...tickets, savedTicket])
            setSubject('')
            setDescription('')
            //console.log(result)
        }
        catch (error) {
            console.error('Failed to create ticket', error)
            alert("Could not create ticket. Please try again.")
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

            <h3>Report a problem</h3> <br />

            <div>
                <input
                    type="text"
                    value={subject}
                    onChange={(e) => setSubject(e.target.value)}
                    placeholder='subject'
                /> <br />

                <input
                    type="text"
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                    placeholder='description'
                />
            </div> <br />

            <button onClick={createTicket}>Report</button> <br />

            <ul>
                {tickets.map(t => (<li key={t.ticketId}>
                    {t.ticketId},
                    {t.subject},
                    {t.description},
                    {t.status}
                </li>))}
            </ul>
        </>
    )
}