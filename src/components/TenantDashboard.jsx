import { useState } from 'react'


export const TenantDashboard = ({ name, tickets, setTickets, isLoading }) => {


    const [subject, setSubject] = useState('')
    const [description, setDescription] = useState('')

    async function handleAddTicket() {
        const url = "https://localhost:7130/Tickets"

        if (subject === '' || description === '') {
            return alert("The fields should not be empty")
        }

        const newTicket = {
            tenantId: 1,
            subject: subject,
            description: description,
            status: "Not Fixed"
        }

        try {
            const response = await fetch(url, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(newTicket)
            })

            if (!response.ok) {
                throw new Error(`Response Status: ${response.status}`)
            }

            const savedTicket = await response.json()
            setTickets([...tickets, savedTicket])
            setSubject('')
            setDescription('')
            //console.log(result)

        }
        catch {
            console.error('failed')
        }


    }

    /*Fetching mockData: This method do not change the data in the state. It 
    prevents data loss if the user write while the mock data is being fetched.
    useEffect(() => {

        const timer = setTimeout(() => {
            setTickets(prev => [...prev, ...mockData])
        }, 7000)

       return () => {
        clearTimeout(timer)
       }
    },[]);*/

    /*this verify if the value of isLoading,
    if its true, it will always show the loading spinner*/

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

            <button onClick={handleAddTicket}>Report</button> <br />

            <ul>
                {tickets.map(t => (<li key={t.ticketId}>
                    {t.ticketId},
                    {t.tenantId},
                    {t.subject},
                    {t.description},
                    {t.status}
                </li>))}
            </ul>
        </>
    )
}