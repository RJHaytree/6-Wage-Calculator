var date = new Date();
var time = date.getHours();
var welcome = String;

if (time < 11) {
    welcome = 'Good Morning, Student!';
}
else if (time < 18) {
    welcome = 'Good Afternoon, Student!';
}
else if (time < 23) {
    welcome = 'Good Evening, Student!';
}
else {
    welcome = 'Welcome, Student!';
}

document.write(welcome);