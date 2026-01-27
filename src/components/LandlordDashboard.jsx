
export const LandlordDashboard = ({name,tickets, fixTicket}) => {

    return (
        <>
           <h2>Hello {name} </h2>

           <ul>
               {tickets.map(t => (<li key= {t.ticket_id}>
                {t.ticket_id}, 
                {t.tenant_id}, 
                {t.subject}, 
                {t.description},
                {t.status},   <button onClick={() => fixTicket(t.ticket_id)}>Mark as fixed</button>
               </li>))}
           </ul> <br />

          

           <p>{tickets.status}</p>

        </>
        

        
    )
}