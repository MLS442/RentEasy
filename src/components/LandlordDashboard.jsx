
export const LandlordDashboard = ({name,tickets, handleFixTicket,isLoading,tenants,properties, deleteTicket}) => {

        if(isLoading){
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
                {t.tenantId},
                {t.tenant?.fullName},
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