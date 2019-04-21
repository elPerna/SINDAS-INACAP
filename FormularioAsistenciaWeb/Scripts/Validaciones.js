
//function lettersOnly(evt) {
//    evt = (evt) ? evt : event;
//    var charCode = (evt.charCode) ? evt.charCode : ((evt.keyCode) ? evt.keyCode :
//        ((evt.which) ? evt.which : 0));
//    if (charCode > 31 && (charCode < 65 || charCode > 90) &&
//        (charCode < 97 || charCode > 122)) {
//        alert("Por favor ingrese solo letras.");
//        return false;
//    }
//    return true;
//}

function lettersOnly(e) {
        key = e.keyCode || e.which;

    teclado=String.fromCharCode(key).toLowerCase();

    letras="qwertyuiopasdfghjklñzxcvbnm ";

    especiales="8-37-38-46-164";

    teclado_especial=false;

        for(var i in especiales){
            if(key==especiales[i]){
        teclado_especial = true;
    break;
}
}

        if(letras.indexOf(teclado)==-1 && !teclado_especial){
            return false;
}
}



//Valida RUT
var Fn = {
    // Valida el rut con su cadena completa "XXXXXXXX-X"
    validaRut: function (rutCompleto) {
        if (!/^[0-9]+[-|‐]{1}[0-9kK]{1}$/.test(rutCompleto))
            return false;
        var tmp = rutCompleto.split('-');
        var digv = tmp[1];
        var rut = tmp[0];
        if (digv == 'K') digv = 'k';
        return (Fn.dv(rut) == digv);
    },
    dv: function (T) {
        var M = 0, S = 1;
        for (; T; T = Math.floor(T / 10))
            S = (S + T % 10 * (9 - M++ % 6)) % 11;
        return S ? S - 1 : 'k';
    }
}
//$("#btnRegistrar").click(function () {
//    if (Fn.validaRut($("#txtRut").val())) {
//        $("#lblmsg").html("El rut ingresado es válido :D");
//    } else {
//        $("#lblmsg").html("El Rut no es válido :'( ");
//    }
//});

//function onRutBlur(obj) {
//    if (VerificaRut(obj.value))
//        alert("Rut correcto");
//    else
//        alert("Rut incorrecto");
//}


//function VerificaRut(rut) {
//    if (rut.toString().trim() != '' && rut.toString().indexOf('-') > 0) {
//        var caracteres = new Array();
//        var serie = new Array(2, 3, 4, 5, 6, 7);
//        var dig = rut.toString().substr(rut.toString().length - 1, 1);
//        rut = rut.toString().substr(0, rut.toString().length - 2);

//        for (var i = 0; i < rut.length; i++) {
//            caracteres[i] = parseInt(rut.charAt((rut.length - (i + 1))));
//        }

//        var sumatoria = 0;
//        var k = 0;
//        var resto = 0;

//        for (var j = 0; j < caracteres.length; j++) {
//            if (k == 6) {
//                k = 0;
//            }
//            sumatoria += parseInt(caracteres[j]) * parseInt(serie[k]);
//            k++;
//        }

//        resto = sumatoria % 11;
//        dv = 11 - resto;

//        if (dv == 10) {
//            dv = "K";
//        }
//        else if (dv == 11) {
//            dv = 0;
//        }

//        if (dv.toString().trim().toUpperCase() == dig.toString().trim().toUpperCase())
//            return true;
//        else
//            return false;
//    }
//    else {
//        return false;
//    }
//}