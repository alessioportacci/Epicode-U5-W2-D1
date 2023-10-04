$(document).ready(function ()
{

    function getStatoHTML(stato)
    {
        let bg = " bg-info "
        if (stato == "In consegna")
            bg = " bg-primary "
        else if (stato == "Consegnato")
            bg = " bg-success "

        return "<button type=\"button\" class=\"w-100 btn btn-light position-relative\">" + 
                    stato + 
                    "<span class=\"position-absolute top-0 start-100 translate-middle p-2" + bg + "border border-light rounded-circle\">" +
                        "<span class=\"visually-hidden\"> New alerts</span>" +
                    "</span>" +
                    "</button> "

    }

    function getLista(user, citta, cliente)
    {
        $("#tbody").empty()
        $.ajax
        ({
            method: 'POST',
            url: "SpedizioniFilter",
            data:
            {
                User: user,
                Citta: citta,
                Cliente: cliente
            },
            success: function (spedizioni)
            {
                $("#nSpedizioni").text(spedizioni.length)
                $.each(spedizioni, function (i, v) {
                    let tr = "<tr>" +
                                "<th>" + v.PkSpedizione + "</th>" +
                                "<td>" + v.CFDestinatario + "</td>" +
                                "<td>" + v.NomeMittente + "</td>" +
                                "<td>" + v.DataSpedizione + "</td>" +
                                "<td>" + v.DataPrevista + "</td>" +
                                "<td>" + v.Destinazione + "</td>" +
                                "<td>" + getStatoHTML(v.Stato) + "</td>" +
                                "<td> <a href='/Spedizione/Aggiornamenti/" + v.PkSpedizione + "'>Visualizza aggiornamenti</a> </td>" +
                            "<tr>"
                    $("#tbody").append(tr);
                })
            }
        })
    }

    //La chiamo immediatamente per riempire la tabella
    if (u === "admin")
        u = ""

    getLista(u, "", "")

    $("#Cerca").click(function ()
    {
        let citta = $("#Citta").val()
        let cliente = $("#Cliente").val()
        getLista(citta, cliente)
    })

})