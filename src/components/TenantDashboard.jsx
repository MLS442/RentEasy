import { useState } from 'react'

export const TenantDashboard  = ({name,tickets,setTickets}) => {

    
    const [subject, setSubject] = useState('')
    const [description, setDescription] = useState('')

    const handleAddTicket = () => {
        if(subject === '' || description === ''){
           return alert("The fields should not be empty")
        }

        const newTicket = {ticket_id: crypto.randomUUID(), 
            tenant_id: Date.now(), 
            subject: subject, 
            description: description,
            status: "Not Fixed"
        }

        setTickets([...tickets, newTicket])
        setSubject('')
        setDescription('')

    }

    return (
        <>
           <h2>Hello {name} </h2>

           <h3>Report a problem</h3> <br />

           <div>
               <input 
               type="text"
               value= {subject}
               onChange={(e) => setSubject(e.target.value) }
               placeholder='subject'
               /> <br />

               

               <input 
               type="text"
               value= {description}
               onChange={(e) => setDescription(e.target.value) }
               placeholder='description'
               />
           </div> <br />

           <button onClick={handleAddTicket}>Report</button> <br />

           <ul>
               {tickets.map(t => (<li key= {t.ticket_id}>
                {t.ticket_id}, 
                {t.tenant_id}, 
                {t.subject}, 
                {t.description},
                {t.status}
               </li>))}
           </ul>
        </>
    )
}