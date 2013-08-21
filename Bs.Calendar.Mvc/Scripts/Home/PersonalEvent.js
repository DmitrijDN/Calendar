﻿function PersonalEventVm() {
    var self = this;

    var dateTimeControl = {
        fromTime: $("#fromTime"),
        toTime: $("#toTime"),
        date: $("#date")
    }; //Setup time range control html elements

    var formatSettings = { date: "YYYY-MM-DD", time: "hh:mm a" };
    var dateDefaults = { initialValue: moment().startOf('day'), initialDifference: { minutes: dateTimeControl.fromTime.timepicker('option', 'step') } };

    var setTime = function (updateMoment, withMoment) {

        return updateMoment
            .hour(withMoment.hour())
            .minute(withMoment.minute())
            .second(withMoment.second());
    };
    var setDate = function (updateMoment, withMoment) {

        return updateMoment
            .year(withMoment.year())
            .month(withMoment.month())
            .date(withMoment.date());
    };

    self.fromDateTime = ko.observable(dateDefaults.initialValue);
    self.toDateTime = ko.observable(dateDefaults.initialValue.add(dateDefaults.initialDifference));

    self.isAllDay = ko.observable(false);

    self.dateInput = {
        value: ko.computed(function() {
            return self.fromDateTime().format(formatSettings.date);
        }, self),
        min: moment().format(formatSettings.date),
        dateChanged: function (context, event) {

            var dateString = $(event.target).val();

            if (dateString == "") {
                return;
            }

            var newDate = moment(dateString).startOf('day'),
                currentDate = self.fromDateTime(),
                minDate = moment(self.dateInput.min).startOf('day');

            if (newDate > minDate) {
                setDate(currentDate, newDate);

                self.fromDateTime(currentDate);
                self.toDateTime(currentDate);
            }
            else {
                $(event.target).val(self.dateInput.value());
            }
        }
    };

    self.fromInput = {
        value: ko.computed(function () {
                return self.fromDateTime().format(formatSettings.time);
        }, self),
        timeChanged: function (context, event) {
            var fromTime = moment($(event.target).val(), formatSettings.time),
                currentFromTime = self.fromDateTime();

            //Check specified time format

            if (!moment.isMoment(fromTime) ||
                !fromTime.isValid() ||
                event.type === "timeFormatError") {
                self.toDateTime(currentFromTime);
                return;
            }

            //Update current "to" time and "from" time if needed

            var toTime = moment(self.toDateTime().format(formatSettings.time), formatSettings.time);

            setTime(currentFromTime, fromTime);
            self.fromDateTime(currentFromTime);
            
            if (fromTime > toTime) {
                self.toDateTime(currentFromTime);
            }
        }
    };    

    self.toInput = {
        value: ko.computed(function () {
            return self.toDateTime().format(formatSettings.time);
        }, self),
        timeChanged: function (context, event) {
            //Todo: This bidlocodishe is because of time formats difference in case of using timepicker and moment.js
            var toTime = moment($(event.target).val(), formatSettings.time), //moment($(event.target).timepicker("getTime")).year(0).month(0).date(1),
                currentToTime = self.toDateTime();

            //Check specified time format

            if (!moment.isMoment(toTime) ||
                !toTime.isValid() ||
                event.type === "timeFormatError") {
                self.toDateTime(currentToTime);
                return;
            }

            //Update current "to" time and "from" time if needed

            var fromTime = moment(self.fromDateTime().format(formatSettings.time), formatSettings.time); //self.fromDateTime();

            setTime(currentToTime, toTime);
            self.toDateTime(currentToTime);

            if (toTime < fromTime) {
                self.fromDateTime(currentToTime);
            }
        }
    };
};