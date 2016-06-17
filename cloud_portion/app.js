// Andrew M. Calhoun
// CS 496-400 HW 3B
// Web API

var http = require('http');
var express = require('express');
var request = require('request');
var path = require('path');
var session = require('express-session');
var handlebars = require('express-handlebars').create({defaultLayout:'main'});

var bodyparser = require('body-parser');
var cookieparser = require('cookie-parser');
var logger = require('morgan');

var rootURL = 'http://52.27.116.225:8003';
var app = express();

app.use(express.static(__dirname + '/views/'));
app.use(express.static(__dirname + '/views/layouts/'));
app.use(express.static(__dirname + '/views/img/'));
app.use(express.static(__dirname + '/helpers/'));
app.use(express.static(__dirname + '/public/')); // should I need to add public members.
app.use(express.static(__dirname + '/routes/'));

var routes = require('./routes/index.js');
var drinks = require('./routes/drinks.js');
var api = require('./routes/api.js');

var mongojs = require('mongojs');
var db = mongojs('pairings', ['food', 'beer', 'wine', 'spirits', 'user']);

var MongoClient = require('mongodb').MongoClient;
var assert = require('assert');

var allowCrossDomain = function(res, req, next) { 
	res.header('Access-Control-Allow-Origin', '*');
	res.header('Access-Control-Allow-Methods', 'GET, PUT, POST, DELETE, OPTIONS');
	res.header('Access-Control-Allow-Headers', 'Content-Type, Authorization, Content-Length, X-Requested-With');

	if('OPTIONS' == req.method) { res.send(200); }
	else { next(); }

};


// var ObjectId = require('mongodb').ObjectID;

var url = 'mongodb://localhost:27017/test';
MongoClient.connect(url, function(err, db) {
	assert.equal(null, err);
	console.log("Connected to MongoDB Server.");
	db.close();
});

// TEST FOR THE DRINK LIST AND PAIRING
/*var insertDocument1 = function(db, callback) { 
		db.collection('beer').insertOne(
		{
			"name" : "Tecate Light",
			"type" : "Light Beer",
			"variety" : "Mexican-style Light Pilsner",
			"alccontent" : "4.7",
			"pairing" : "Summer Barbeque Foods, Party Snacks, Hamburgers",
			"flavorprofile" : "Light, slightly sweet. Kind of gross when warm."
		}, function(err, result) {
			assert.equal(err, null);
			console.log("Inserted into the beer collection");
			console.log('beer');
			callback();
	});
};

var insertDocument2 = function(db, callback) { 
		db.collection('spirits').insertOne(
		{
			"name" : "Mezcali",
			"type" : "Tequila",
			"variety" : "High Flavor Tequila",
			"alccontent" : "40",
			"pairing" : "Chocolate, cheese, meats",
			"flavorprofile" : "Smoky and wonderful."
		}, function(err, result) {
			assert.equal(err, null);
			console.log("Inserted into the spirits collection");
			callback();
	});
};

var insertDocument3 = function(db, callback) { 
		db.collection('wine').insertOne(
		{
			"name" : "Yellowtail Shiraz",
			"type" : "Table Wine",
			"variety" : "Bargain Red Wine",
			"alccontent" : "12.5",
			"pairing" : "Red meat, BBQ, salmon, chicken, anything really.",
			"flavorprofile" : "Surprisingly tasty..."
		}, function(err, result) {
			assert.equal(err, null);
			console.log("Inserted into the wine collection");
			callback();
	});
};

var insertDocument4 = function(db, callback) { 
		db.collection('food').insertOne(
		{
			"name" : "Brie",
			"type" : "Soft Cheese",
			"pairs with" : "White Wine, Red Wine, Scotch, Non-Potato Vodkas, Whiskey, Port, Sweet Wines",
		}, function(err, result) {
			assert.equal(err, null);
			console.log("Inserted into the food collection");
			callback();
	});
};
*/
/*MongoClient.connect(url, function(err, db) {
	assert.equal(null, err);
	insertDocument1(db, function() {
		db.close();
	});
});

MongoClient.connect(url, function(err, db) {
	assert.equal(null, err);
	insertDocument2(db, function() {
		db.close();
	});
});

MongoClient.connect(url, function(err, db) {
	assert.equal(null, err);
	insertDocument3(db, function() {
		db.close();
	});
});

MongoClient.connect(url, function(err, db) {
	assert.equal(null, err);
	insertDocument4(db, function() {
		db.close();
	});
});*/


// Things are added to the mongo...

app.engine('handlebars', handlebars.engine);
app.set('view engine', 'handlebars');
app.set('port', 8000);

// var connection = require('./connection.js');

app.use(allowCrossDomain);
app.use(logger('combined'));
app.use(bodyparser.json( { limit: '50mb' } ));
app.use(bodyparser.urlencoded( { limit: '50mb' } ));

app.use('/', routes);
app.use('/drinks', drinks);
app.use('/api', api);


// error 404
app.use(function(err, req, res, next){
	res.status(404);
	res.render('404');
	next(err);
});


//error 500
app.use(function(err, req, res, next){
	console.error(err.stack);
	res.type('plain/text');
	res.status(500);
	res.render('500', {
		message: err.message,
		error: err
	});
});

app.listen(app.get('port'), function() {
	console.log('Express Server started on http://52.27.116.225:' + app.get('port') + '...press ctrl-c to quit.');
});

module.exports = app;