async function FixTicket(ticketToUpdate) {
    const url = `https://localhost:7130/Tickets/${ticketToUpdate.ticketId}`

    delete ticketToUpdate.tenant

    const token = localStorage.getItem('token')

    const response = await fetch(url, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(ticketToUpdate),
    })

    return response
}

async function deleteTicket(id) {
    const url = `https://localhost:7130/Tickets/${id}`
    const token = localStorage.getItem('token')

    const response = await fetch(url, {
        method: "DELETE",
        headers: {
            'Authorization': `Bearer ${token}`
        }
    })

    return response
}

async function getTickets() {
    const url = "https://localhost:7130/Tickets"
    const token = localStorage.getItem('token')

    const response = await fetch(url, {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    })

    return response
}

async function postTicket(newTicket) {
    const url = "https://localhost:7130/Tickets"
    const token = localStorage.getItem('token')

    const response = await fetch(url, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(newTicket)
    })

    return response
}

const TicketService = {FixTicket, deleteTicket, getTickets, postTicket};
export default TicketService;