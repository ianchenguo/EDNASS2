$(document).ready(function () {
    AlertOny();
    SelectedOptionValue();
});

function AlertOny() {
    alert("AgentDoctor");
};

function SelectedOptionValue() {
    var selectedList = $('select#AgentDoctorPackageRegisterPackageTypeDropDwonList');
    // var selectedOpt = $('select#AgentDoctorPackageRegisterPackageTypeDropDwonList option:selected');
    var sampleValue = $('div.agent-doctor-sample-selected-value');

    selectedList.on('change', function () {
        var hasValue = selectedList.has('[selected]');
        var selectedOpt = $('select#AgentDoctorPackageRegisterPackageTypeDropDwonList option:selected');
        var inputDisplay = $('#AgentDoctorRegisterFormExpireDateInput');

        if (hasValue) {
            // var selectedValueId = selectedOpt.val();
            var seleOptExpDateElem = $('option.Expire-date-option-second');
            seleOptExpDateElem.css('display', 'none');
            var selectedOpVal = seleOptExpDateElem.val();
            console.log('selectedOpVal ==== '+selectedOpVal);

            //Id is string date, sample is "2017-06-24". 'yyyy-MM-dd'
            var type = $.type(selectedOpVal);
            // var expireDate333 = selectedOpVal;

            var formatExpiredDate = formatDateFunc (selectedOpVal);
            console.log('ed is '+formatExpiredDate);
            inputDisplay.val(formatExpiredDate);

            sampleValue.html('value is: ' + selectedOpVal);

            // $.ajax({
            //     type: "POST",
            //     url: "GetTestJsonData",
            //     data: selectedOpVal,
            //     contentType: "application/json; charset=utf-8",
            //     dataType: "json",
            //     success: function (msg) {
            //         alert("Name = " + msg.Name);
            //     }
            // });
        } else {
            // sampleValue.html('No Value  ------------------------div ');
            console.log("wocao nothing.");
        };
    });

    // var gan = $('.wocaoselected');

}

function formatDateFunc (optExpDate) {
    var expireDate = optExpDate;
    var typeD = $.type(expireDate);
    console.log("passed expireDate: "+expireDate);
    var n = expireDate.indexOf(" ");
    var shortExpiredDate = expireDate.slice(0, n);
    console.log('shortExpiredDate length'+shortExpiredDate.length);
    var ExpiredDate1 = shortExpiredDate.replace('/', '-');
    var n1 = ExpiredDate1.indexOf("-");
    var n2 = ExpiredDate1.indexOf("/");
    var Month = ExpiredDate1.slice(n1, n2);
    console.log('Month is '+Month+" n1 "+n1+" n2 "+n2+" Length: "+Month.length);
    if (Month.length<3 && Month.length > 0) {
        console.log("Month[1] is: -- "+Month[1]);
        var sigMonth = Month[1];
        var completedMonth = "-0"+sigMonth;
        console.log('completedMonth ==== '+completedMonth);
        var eDate1 = ExpiredDate1.replace(Month, completedMonth);
        console.log("eDate1 ==== "+eDate1);
    };



    var ExpiredDate2 = eDate1.replace('/', '-');
    console.log('ExpiredDate2 is ==== '+ExpiredDate2);

    return ExpiredDate2;
}