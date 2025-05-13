function Register() {
    if (document.getElementById('userpassword').value === document.getElementById('userreppassword').value) {
        $.ajax({
            type: "POST",
            url: `/api/Users/Add/${document.getElementById('useremail').value}/${document.getElementById('userpassword').value}/${document.getElementById('userfirstname').value}/${document.getElementById('userlastname').value}`,
            contentType: "application/json",
            dataType: "json",
            success: function () {
                Swal.fire({
                    title: "Успешная регистрация!",
                    confirmButtonColor: "#0d6efd",
                }).then(() => {
                    window.location.href = '/Access/Authorization';
                });
            }
        })
    }
    else {
        Swal.fire({
            icon: "error",
            title: "Пароли различаются!",
            confirmButtonColor: "#0d6efd",
        });
    }
};

function Auth() {
    if (document.getElementById('userpassword').value !== "" && document.getElementById('useremail').value !== "") {     
        $.ajax({
            type: "GET",
            url: `/api/Users/Login/${document.getElementById('useremail').value}/${document.getElementById('userpassword').value}`,
            contentType: "application/json",
            dataType: "json",
            success: function (user) {
                if (user.statusCode === 200) {
                    Swal.fire({
                        icon: "success",
                        title: `Успешная авторизация - ${user.value.lastName} ${user.value.firstName}`,
                        confirmButtonColor: "#0d6efd",
                    }).then(() => {
                        if (user.value.role === "Администратор") {
                            window.location.href = '/Member/Index';
                        }
                        else if (user.value.role === "Менеджер") {
                            window.location.href = '/Customer/Index';
                        }
                        else if (user.value.role === "Оператор") {
                            window.location.href = '/Schedule/List';
                        }
                        else if (user.value.role === "Рабочий") {
                            window.location.href = '/Worker/Main';
                        }
                    });
                }
                else {
                    Swal.fire({
                        icon: "error",
                        title: "Пользователь не найден!",
                        confirmButtonColor: "#0d6efd",
                    });
                }
            }
        })
    }
    else {
        Swal.fire({
            icon: "error",
            title: "Заполните все поля!",
            confirmButtonColor: "#0d6efd",
        });
    }
};

function OpenMap(wellid, latitude, longitude) {
    var mapDiv = document.createElement("div");
    mapDiv.id = `map${wellid}`;
    mapDiv.style.width = "100%";
    mapDiv.style.height = "300px";

    latitude = latitude.replace(',', '.');
    longitude = longitude.replace(',', '.');

    document.getElementById(`openmap${wellid}`).insertAdjacentElement("afterend", mapDiv);

    var map;
    DG.then(function () {
        map = DG.map(`map${wellid}`, {
            center: [latitude, longitude],
            zoom: 13
        });

        DG.marker([latitude, longitude]).addTo(map).bindPopup('Вы кликнули по мне!');
    });

    document.getElementById(`hidemap${wellid}`).style.display = 'block';
    document.getElementById(`openmap${wellid}`).style.display = 'none';
};

function HideMap(wellid) {
    var element = document.getElementById(`map${wellid}`);
    if (element) {
        element.parentNode.removeChild(element);

        document.getElementById(`hidemap${wellid}`).style.display = 'none';
        document.getElementById(`openmap${wellid}`).style.display = 'block';
    }
};

function openModalCreateWell(options) {
    const url = options.url;
    const modal = $('#modal');

    $.ajax({
        type: 'GET',
        url: url,
        success: function (response) {
            $('.modal-dialog');
            modal.find(".modal-body").html(response);
            modal.modal('show');
        },
        failure: function () {
            modal.modal('hide');
        }
    }).then(() => {
        let containerElement = document.querySelector('#modal');
        containerElement.setAttribute('style', 'display: flex !important');
    });
};

function openModalDisplayWell(options) {
    const url = options.url;
    const wellid = options.wellid;
    const modal = $('#modal');

    $.ajax({
        type: 'GET',
        url: url,
        data: { "wellid": wellid },
        success: function (response) {
            $('.modal-dialog');
            modal.find(".modal-body").html(response);
            modal.modal('show');
        },
        failure: function () {
            modal.modal('hide');
        }
    }).then(() => {
        let containerElement = document.querySelector('#modal');
        containerElement.setAttribute('style', 'display: flex !important');
    });
};

function openModalCreateClient(options) {
    const url = options.url;
    const modal = $('#modal');

    $.ajax({
        type: 'GET',
        url: url,
        success: function (response) {
            $('.modal-dialog');
            modal.find(".modal-body").html(response);
            modal.modal('show');
        },
        failure: function () {
            modal.modal('hide');
        }
    }).then(() => {
        let containerElement = document.querySelector('#modal');
        containerElement.setAttribute('style', 'display: flex !important');
    });
};

function openModalCreateOrder(options) {
    const url = options.url;
    const modal = $('#modal');

    $.ajax({
        type: 'GET',
        url: url,
        success: function (response) {
            $('.modal-dialog');
            modal.find(".modal-body").html(response);
            modal.modal('show');
        },
        failure: function () {
            modal.modal('hide');
        }
    }).then(() => {
        let containerElement = document.querySelector('#modal');
        containerElement.setAttribute('style', 'display: flex !important');
    });
};

function openModalEditUser(options) {
    const url = options.url;
    const userid = options.userid;
    const modal = $('#modal');

    $.ajax({
        type: 'GET',
        url: url,
        data: { "userid": userid },
        success: function (response) {
            $('.modal-dialog');
            modal.find(".modal-body").html(response);
            modal.modal('show');
        },
        failure: function () {
            modal.modal('hide');
        }
    }).then(() => {
        let containerElement = document.querySelector('#modal');
        containerElement.setAttribute('style', 'display: flex !important');
    });
};

function openModalAddUser(options) {
    const url = options.url;
    const modal = $('#modal');

    $.ajax({
        type: 'GET',
        url: url,
        success: function (response) {
            $('.modal-dialog');
            modal.find(".modal-body").html(response);
            modal.modal('show');
        },
        failure: function () {
            modal.modal('hide');
        }
    }).then(() => {
        let containerElement = document.querySelector('#modal');
        containerElement.setAttribute('style', 'display: flex !important');
    });
};

function openModalDisplayClient(options) {
    const url = options.url;
    const clientid = options.clientid;
    const modal = $('#modal');

    $.ajax({
        type: 'GET',
        url: url,
        data: { "customerid": clientid },
        success: function (response) {
            $('.modal-dialog');
            modal.find(".modal-body").html(response);
            modal.modal('show');
        },
        failure: function () {
            modal.modal('hide');
        }
    }).then(() => {
        let containerElement = document.querySelector('#modal');
        containerElement.setAttribute('style', 'display: flex !important');
    });
};

function openModalDisplayWorker(options) {
    const url = options.url;
    const workerid = options.userid;
    const modal = $('#modal');

    $.ajax({
        type: 'GET',
        url: url,
        data: { "workerid": workerid },
        success: function (response) {
            $('.modal-dialog');
            modal.find(".modal-body").html(response);
            modal.modal('show');
        },
        failure: function () {
            modal.modal('hide');
        }
    }).then(() => {
        let containerElement = document.querySelector('#modal');
        containerElement.setAttribute('style', 'display: flex !important');
    });
};

function openModalDisplaySchedule(options) {
    const url = options.url;
    const schedileid = options.scheduleid;
    const modal = $('#modal');

    $.ajax({
        type: 'GET',
        url: url,
        data: { "workscheduleid": schedileid },
        success: function (response) {
            $('.modal-dialog');
            modal.find(".modal-body").html(response);
            modal.modal('show');
        },
        failure: function () {
            modal.modal('hide');
        }
    }).then(() => {
        let containerElement = document.querySelector('#modal');
        containerElement.setAttribute('style', 'display: flex !important');
    });
}

function openModalDisplayOrder(options) {
    const url = options.url;
    const orderid = options.orderid;
    const modal = $('#modal');

    $.ajax({
        type: 'GET',
        url: url,
        data: { "orderid": orderid },
        success: function (response) {
            $('.modal-dialog');
            modal.find(".modal-body").html(response);
            modal.modal('show');
        },
        failure: function () {
            modal.modal('hide');
        }
    }).then(() => {
        let containerElement = document.querySelector('#modal');
        containerElement.setAttribute('style', 'display: flex !important');
    });
}

function openModalDisplayNotification(options) {
    const url = options.url;
    const notificationid = options.notificationid;
    const modal = $('#modal');

    $.ajax({
        type: 'GET',
        url: url,
        data: { "notificationid": notificationid },
        success: function (response) {
            $('.modal-dialog');
            modal.find(".modal-body").html(response);
            modal.modal('show');
        },
        failure: function () {
            modal.modal('hide');
        }
    }).then(() => {
        let containerElement = document.querySelector('#modal');
        containerElement.setAttribute('style', 'display: flex !important');
    });
}

function AddWell() {
    const fields = ['name', 'latitude', 'longitude', 'depth', 'diameter'];
    let allFieldsFilled = true;

    fields.forEach(field => {
        const input = document.getElementById(field);
        const errorElement = document.getElementById(`${field}-error`);

        if (input.value === "") {
            errorElement.style.display = "block";
            allFieldsFilled = false;
        } else {
            errorElement.style.display = "none";
        }
    });

    if (allFieldsFilled) {
        $.ajax({
            type: "POST",
            url: `/api/Wells/Add/${document.getElementById('name').value}/${document.getElementById('latitude').value}/${document.getElementById('longitude').value}/${document.getElementById('depth').value}/${document.getElementById('diameter').value}/${document.getElementById('drillingmethod').value}/${document.getElementById('soiltype').value}`,
            contentType: "application/json",
            dataType: "json",
            success: function (well) {
                Swal.fire({
                    icon: "success",
                    title: `Скважина ${well.value.name} успешно добавлена!`,
                    confirmButtonColor: "#0d6efd",
                }).then(() => {
                    location.reload();
                });
            }
        })
    }
};

function EditWell() {
    const fields = ['name', 'latitude', 'longitude', 'depth', 'diameter'];
    let allFieldsFilled = true;

    fields.forEach(field => {
        const input = document.getElementById(field);
        const errorElement = document.getElementById(`${field}-error`);

        if (input.value === "") {
            errorElement.style.display = "block";
            allFieldsFilled = false;
        } else {
            errorElement.style.display = "none";
        }
    });

    if (allFieldsFilled) {
        $.ajax({
            type: "POST",
            url: `/api/Wells/Edit/${document.getElementById('wellidinedit').value}/${document.getElementById('name').value}/${document.getElementById('latitude').value}/${document.getElementById('longitude').value}/${document.getElementById('depth').value}/${document.getElementById('diameter').value}/${document.getElementById('drillingmethod').value}/${document.getElementById('soiltype').value}`,
            contentType: "application/json",
            dataType: "json",
            success: function (well) {
                Swal.fire({
                    icon: "success",
                    title: `Скважина ${well.name} успешно обновлена!`,
                    confirmButtonColor: "#0d6efd",
                }).then(() => {
                    location.reload();
                });
            }
        })
    }
};

function AddClient() {
    const fields = ['lastname', 'firstname', 'contactnumber', 'email', 'address'];
    let allFieldsFilled = true;

    fields.forEach(field => {
        const input = document.getElementById(field);
        const errorElement = document.getElementById(`${field}-error`);

        if (input.value === "") {
            errorElement.style.display = "block";
            allFieldsFilled = false;
        } else {
            errorElement.style.display = "none";
        }
    });

    if (allFieldsFilled) {
        $.ajax({
            type: "POST",
            url: `/api/Clients/Add/${document.getElementById('lastname').value}/${document.getElementById('firstname').value}/${document.getElementById('contactnumber').value}/${document.getElementById('email').value}/${document.getElementById('address').value}/${document.getElementById('clienttype').value}`,
            contentType: "application/json",
            dataType: "json",
            success: function (client) {
                Swal.fire({
                    icon: "success",
                    title: `Клиент ${client.lastName} ${client.firstName} успешно добавлен!`,
                    confirmButtonColor: "#0d6efd",
                }).then(() => {
                    location.reload();
                });
            }
        })
    }
};

function AddWorkOrder() {
    const fields = ['orderdate', 'description', 'cost'];
    let allFieldsFilled = true;

    fields.forEach(field => {
        const input = document.getElementById(field);
        const errorElement = document.getElementById(`${field}-error`);

        if (input.value === "") {
            errorElement.style.display = "block";
            allFieldsFilled = false;
        } else {
            errorElement.style.display = "none";
        }
    });
    if (allFieldsFilled) {
        $.ajax({
            type: "POST",
            url: `/api/WorkOrders/Add/${document.getElementById('client').value}/${document.getElementById('well').value}/${document.getElementById('orderdate').value}/${document.getElementById('description').value}/${document.getElementById('cost').value}`,
            contentType: "application/json",
            dataType: "json",
            success: function () {
                Swal.fire({
                    icon: "success",
                    title: `Заказ на бурение успешно добавлен!`,
                    confirmButtonColor: "#0d6efd",
                }).then(() => {
                    location.reload();
                });
            }
        })
    }
};

function AddUser() {
    const fields = ['email', 'password', 'lastname', 'firstname'];
    let allFieldsFilled = true;

    fields.forEach(field => {
        const input = document.getElementById(field);
        const errorElement = document.getElementById(`${field}-error`);

        if (input.value === "") {
            errorElement.style.display = "block";
            allFieldsFilled = false;
        } else {
            errorElement.style.display = "none";
        }
    });

    if (allFieldsFilled) {
        $.ajax({
            type: "POST",
            url: `/api/Users/Add/${document.getElementById('email').value}/${document.getElementById('password').value}/${document.getElementById('firstname').value}/${document.getElementById('lastname').value}/${document.getElementById('role').value}`,
            contentType: "application/json",
            dataType: "json",
            success: function (user) {
                console.log(user);
                Swal.fire({
                    icon: "success",
                    title: `Пользователь ${user.value.username} успешно добавлен!`,
                    confirmButtonColor: "#0d6efd",
                }).then(() => {
                    //location.reload();
                });
            }
        })
    }
};

function EditUser() {
    const fields = ['lastnameedit', 'firstnameedit'];
    let allFieldsFilled = true;

    fields.forEach(field => {
        const input = document.getElementById(field);
        const errorElement = document.getElementById(`${field}-error`);

        if (input.value === "") {
            errorElement.style.display = "block";
            allFieldsFilled = false;
        } else {
            errorElement.style.display = "none";
        }
    });

    if (allFieldsFilled) {
        $.ajax({
            type: "PUT",
            url: `/api/Users/Edit/${document.getElementById('userid').value}/${document.getElementById('roleedit').value}/${document.getElementById('lastnameedit').value}/${document.getElementById('firstnameedit').value}`,
            contentType: "application/json",
            dataType: "json",
            success: function () {
                Swal.fire({
                    icon: "success",
                    title: `Пользователь успешно обновлен!`,
                    confirmButtonColor: "#0d6efd",
                }).then(() => {
                    location.reload();
                });
            }
        })
    }
};

function EditUserInProfile(userid) {
    const fields = ['lastname', 'firstname'];
    let allFieldsFilled = true;

    fields.forEach(field => {
        const input = document.getElementById(field);
        const errorElement = document.getElementById(`${field}-error`);

        if (input.value === "") {
            errorElement.style.display = "block";
            allFieldsFilled = false;
        } else {
            errorElement.style.display = "none";
        }
    });

    if (allFieldsFilled) {
        $.ajax({
            type: "PUT",
            url: `/api/Users/EditInProfile/${userid}/${document.getElementById('lastname').value}/${document.getElementById('firstname').value}`,
            contentType: "application/json",
            dataType: "json",
            success: function () {
                Swal.fire({
                    icon: "success",
                    title: `Ваши данные успешно обновлены!`,
                    confirmButtonColor: "#0d6efd",
                }).then(() => {
                    location.reload();
                });
            }
        })
    }
};

function EditPassword(userid) {
    const fields = ['oldpass', 'newpass'];
    let allFieldsFilled = true;

    fields.forEach(field => {
        const input = document.getElementById(field);
        const errorElement = document.getElementById(`${field}-error`);

        if (input.value === "") {
            errorElement.style.display = "block";
            allFieldsFilled = false;
        } else {
            errorElement.style.display = "none";
        }
    });

    if (allFieldsFilled) {
        $.ajax({
            type: "GET",
            url: `/api/Users/CheckAvailableOldPassword/${userid}/${document.getElementById('oldpass').value}`,
            contentType: "application/json",
            dataType: "json",
            success: function (user) {
                if (user.statusCode === 404) {
                    Swal.fire({
                        icon: "error",
                        title: "Cтарый пароль неверный!",
                        confirmButtonColor: "#0d6efd",
                    });
                }
                else if (user.statusCode === 200) {
                    $.ajax({
                        type: "PUT",
                        url: `/api/Users/EditPassword/${userid}/${document.getElementById('newpass').value}`,
                        contentType: "application/json",
                        dataType: "json",
                        success: function (user) {
                            Swal.fire({
                                icon: "success",
                                title: "Пароль успешно изменен!",
                                confirmButtonColor: "#0d6efd",
                            }).then(() => {
                                location.reload();
                            });
                        }
                    })
                }
            }
        })
    }
};

function EditClient() {
    var clientid = document.getElementById('customeridinedit').value;
    const fields = ['lastname', 'firstname', 'contactnumber', 'email', 'address'];
    let allFieldsFilled = true;

    fields.forEach(field => {
        const input = document.getElementById(field);
        const errorElement = document.getElementById(`${field}-error`);

        if (input.value === "") {
            errorElement.style.display = "block";
            allFieldsFilled = false;
        } else {
            errorElement.style.display = "none";
        }
    });

    if (allFieldsFilled) {
        $.ajax({
            type: "PUT",
            url: `/api/Clients/Edit/${clientid}/${document.getElementById('lastname').value}/${document.getElementById('firstname').value}/${document.getElementById('contactnumber').value}/${document.getElementById('email').value}/${document.getElementById('address').value}/${document.getElementById('clienttype').value}`,
            contentType: "application/json",
            dataType: "json",
            success: function (client) {
                Swal.fire({
                    icon: "success",
                    title: `Клиент ${client.lastName} ${client.firstName} успешно обновлен!`,
                    confirmButtonColor: "#0d6efd",
                }).then(() => {
                    location.reload();
                });
            }
        })
    }
};

function SetScheduleStatus(woid, variable) {
    $.ajax({
        type: "PUT",
        url: `/api/WorkSchedules/SetStatus/${woid}/${variable}`,
        contentType: "application/json",
        dataType: "json",
        success: function () {
            Swal.fire({
                icon: "success",
                title: `Успешно подтверждено выполнение работы!`,
                confirmButtonColor: "#0d6efd",
            }).then(() => {
                location.reload();
            });
        }
    });
};

function OpenWorkerReport(workerid) {
    $.ajax({
        type: "GET",
        url: `/api/Report/WorkerAll/${workerid}`,
        xhrFields: {
            responseType: 'blob'
        },
        success: function (data) {
            var blob = new Blob([data], { type: 'application/pdf' });
            var url = window.URL.createObjectURL(blob);
            window.open(url);
        },
        error: function (xhr, status, error) {
            // Обработка ошибок
            console.error(xhr.responseText);
        }
    });
};



function SwitchBetween(from, to) {
    document.getElementById(from).style.display = 'none';
    document.getElementById(to).style.display = 'block';
};

function SearchOrder() {
    var orderid = document.getElementById('searchwellfield').value;
    if (orderid != "") {
        $.get(`/Order/Index?orderid=${orderid}`).then(_ => {
            window.location.href = `/Order/Index?orderid=${orderid}`;
        });
    }
    else {
        window.location.href = `/Order/Index`;
    }
};

function SearchMember() {
    var query = document.getElementById('searchwellfield').value;
    if (query != "") {
        $.get(`/Member/Index?query=${query}`).then(_ => {
            window.location.href = `/Member/Index?query=${query}`;
        });
    }
    else {
        window.location.href = `/Member/Index`;
    }
}

function GeneratePassword() {
    var length = 8,
        charset = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
        retVal = "";
    for (var i = 0, n = charset.length; i < length; ++i) {
        retVal += charset.charAt(Math.floor(Math.random() * n));
    }
    document.getElementById('password').value = retVal;
};

function Logout() {
    Swal.fire({
        title: "Вы действительно хотите выйти?",
        showCancelButton: true,
        confirmButtonText: "Да",
        confirmButtonColor: "#0d6efd",
        cancelButtonText: "Нет",
        cancelButtonColor: "#dc3741"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "POST",
                url: `/api/Users/Logout`,
                contentType: "application/json",
                dataType: "json",
                success: function () {
                    window.location.href = '/Access/Authorization';
                }
            })
        }
    });  
};

function SetPlannedCost(workorderid) {
    $.ajax({
        type: "GET",
        url: `/api/WorkSchedules/GetPlannedCost/${workorderid}`,
        contentType: "application/json",
        dataType: "json",
        success: function (cost) {
            document.getElementById('plannedcost').innerHTML = cost;
        }
    })
};

async function AddSchedule() {
    var workorder = document.getElementById('workorders').value;
    var plannedcost = document.getElementById('plannedcost').innerHTML;
    var usersids = getSelectValues(document.getElementById('usersids'));
    var startdate = document.getElementById('startdate').value;
    var enddate = document.getElementById('enddate').value;

    const fields = ['usersids', 'startdate', 'enddate'];
    let allFieldsFilled = true;

    fields.forEach(field => {
        const input = document.getElementById(field);
        const errorElement = document.getElementById(`${field}-error`);

        if (input.value === "") {
            errorElement.style.display = "block";
            allFieldsFilled = false;
        } else {
            errorElement.style.display = "none";
        }
    });

    var hasTrueResult = false;

    for (const element of usersids) {
        const result = await $.ajax({
            type: "GET",
            url: `/api/WorkSchedules/CheckAvailableWorker/${element}/${startdate}/${enddate}`,
            contentType: "application/json",
            dataType: "json"
        });

        if (result.value) {
            hasTrueResult = true;
            const user = await $.ajax({
                type: "GET",
                url: `/api/Users/GetUserById/${element}`,
                contentType: "application/json",
                dataType: "json"
            });

            Swal.fire({
                title: `Рабочий ${user.value.lastName} ${user.value.firstName} уже работает в этот промежуток времени`,
                confirmButtonColor: "#0d6efd",
            });
        }
    }

    if (!hasTrueResult) {
        $.ajax({
            type: "POST",
            url: `/api/WorkSchedules/Add/${workorder}/${startdate}/${enddate}/${plannedcost}`,
            contentType: "application/json",
            dataType: "json",
            data: JSON.stringify(usersids),
            success: function () {
                location.href = "/Schedule/List";
            }
        })
    }
};

function getSelectValues(select) {
    var result = [];
    var options = select && select.options;
    var opt;

    for (var i = 0, iLen = options.length; i < iLen; i++) {
        opt = options[i];

        if (opt.selected) {
            result.push(opt.value || opt.text);
        }
    }
    return result;
};

function SetReadyWork(welluserid) {
    $.ajax({
        type: "PUT",
        url: `/api/WorkSchedules/SetReadyByWorker/${welluserid}`,
        contentType: "application/json",
        dataType: "json",
        success: function () {
            Swal.fire({
                icon: "success",
                title: "Отметка о выполнение работы поставлена!",
                confirmButtonColor: "#0d6efd",
            }).then(() => {
                location.reload();
            });
        }
    })
};

function SeachWell() {
    var name = document.getElementById('searchwellfield').value;
    if (name != "") {
        $.get(`/Borehole/Index?wellname=${name}`).then(_ => {
            window.location.href = `/Borehole/Index?wellname=${name}`;
        });
    }
    else {
        window.location.href = `/Borehole/Index`;
    }
};

function SearchCustomer() {
    var namewithsurname = document.getElementById('searchwellfield').value;
    if (namewithsurname != "") {
        $.get(`/Customer/Index?query=${namewithsurname}`).then(_ => {
            window.location.href = `/Customer/Index?query=${namewithsurname}`;
        });
    }
    else {
        window.location.href = `/Borehole/Index`;
    }
};

function RedirectTo(url) {
    window.location.href = url;
}

function SetBlockClient(clientid) {
    $.ajax({
        type: "PUT",
        url: `/api/Clients/IsDeletedTrue/${clientid}`,
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            Swal.fire({
                icon: "success",
                title: `Клиент ${result.value.lastName} ${result.value.firstName} заблокирован!`,
                confirmButtonColor: "#0d6efd",
            }).then(() => {
                location.reload();
            });
        }
    })
}

function SetUnblockClient(clientid) {
    $.ajax({
        type: "PUT",
        url: `/api/Clients/IsDeletedFalse/${clientid}`,
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            Swal.fire({
                icon: "success",
                title: `Клиент ${result.value.lastName} ${result.value.firstName} разблокирован!`,
                confirmButtonColor: "#0d6efd",
            }).then(() => {
                location.reload();
            });
        }
    })
}

function maskPhoneNumber(input) {
    const phoneNumber = input.value.replace(/\D/g, '');
    if (phoneNumber.length <= 10) {
        input.value = phoneNumber.replace(/^(\d{0,3})(\d{0,3})(\d{0,4})/, '($1) $2-$3');
    } else {
        input.value = phoneNumber.replace(/^(\d{0,1})(\d{0,3})(\d{0,3})(\d{0,4})/, '+$1 ($2) $3-$4');
    }
}

function validateTextInput(input) {
    let value = input.value.trim();
    let sanitizedValue = '';
    const invalidChars = new Set('1234567890 !"№;%:?/|*()_+-=.');
    for (let i = 0; i < value.length; i++) {
        if (!invalidChars.has(value[i])) {
            sanitizedValue += value[i];
        }
    }
    sanitizedValue = sanitizedValue.charAt(0).toUpperCase() + sanitizedValue.slice(1).toLowerCase();
    input.value = sanitizedValue;
}

function SetBlockUser() {
    var userid = document.getElementById('userid').value;

    $.ajax({
        type: "PUT",
        url: `/api/Users/Block/${userid}`,
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            Swal.fire({
                icon: "success",
                title: `Пользователь ${result.value.lastName} ${result.value.firstName} заблокирован!`,
                confirmButtonColor: "#0d6efd",
            }).then(() => {
                location.reload();
            });
        }
    })
}

function SetUnblockUser() {
    var userid = document.getElementById('userid').value;

    $.ajax({
        type: "PUT",
        url: `/api/Users/Unblock/${userid}`,
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            Swal.fire({
                icon: "success",
                title: `Пользователь ${result.value.lastName} ${result.value.firstName} разблокирован!`,
                confirmButtonColor: "#0d6efd",
            }).then(() => {
                location.reload();
            });
        }
    });
}

function opendropdownmenu() {
    var mobilemenu = document.getElementById("mobilemenu");
    if (mobilemenu.style.display == "none") {
        mobilemenu.style.display = "block";
    }
    else if (mobilemenu.style.display == "block") {
        mobilemenu.style.display = "none";
    }
}

function SendNotificationToWorker(wsid) {
    $.ajax({
        type: "POST",
        url: `/api/WorkSchedules/SendNotificationToWorker/${wsid}`,
        contentType: "application/json",
        dataType: "json",
        success: function (result) {
            Swal.fire({
                icon: "success",
                title: `Уведомления отправлены!`,
                confirmButtonColor: "#0d6efd",
            }).then(() => {
                location.reload();
            });
        }
    });
}

function GetOrderReport(woid) {
    $.ajax({
        type: "GET",
        url: `/api/Report/Order/${woid}`,
        xhrFields: {
            responseType: 'blob'
        },
        success: function (data) {
            var blob = new Blob([data], { type: 'application/pdf' });
            var url = window.URL.createObjectURL(blob);
            window.open(url);
        },
        error: function (xhr, status, error) {
            // Обработка ошибок
            console.error(xhr.responseText);
        }
    });
}
