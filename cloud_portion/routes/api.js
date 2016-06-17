// Andrew M. Calhoun
// CS496-400
// DUE: 4/24/2016

// MONGOJS Materials: https://www.npmjs.com/package/mongojs
// MONGODB: https://www.mongodb.com/collateral/mongodb-3-2-whats-new?jmp=search&utm_source=google&utm_campaign=US-Brand-Alpha&utm_keyword=mongodb&utm_device=c&utm_network=g&utm_medium=cpc&utm_creative=86031857323&utm_matchtype=e&gclid=CjwKEAjw3fG4BRDsn9GAv7T2zEkSJACNJdjgAOC5N91ZToiCD9X_SBry62ea5D11tFm6yEarQ_-MXxoC6Fjw_wcB

var express = require('express');
var router = express.Router();

var mongojs = require('mongojs');
var mongodb = require('mongodb');
var mongoClient = mongodb.MongoClient;
var db = mongojs('pairings', ['food', 'beer', 'wine', 'spirits', 'user']);

var bodyParser = require('body-parser');
var MongoClient = require('mongodb').MongoClient;
var assert = require('assert');

db.on('error', function(err) {
			console.log('Database Error', err);
});

db.on('connect', function() {
			console.log('Successful connection with mongojs wrapper.');
});

// Get Routes, For Testing

router.get('/', function(req, res) {
	console.log("I have received a GET request for the API.");

	res.status(200);
	res.json("The API is active and humming... GET Route Working Properly.");

});

router.post('/', function(req, res) {
	console.log("I have received a POST request for the API.");

	res.status(200);
	res.json("The API is active and humming... POST Route Working Properly.");

});

router.put('/', function(req, res) {
	console.log("I have received a PUT request for the API.");

	res.status(200);
	res.json("The API is active and humming... PUT Route Working Properly.");

});

router.delete('/', function(req, res) {
	console.log("I have recieved a DELETE request for the API.")

	res.status(200);
	res.json("The API is active and humming... DELETE route working properly.");
});


/*
 [READ]
	GET Routes -- beer
*/

router.get('/beer', function(req, res) {
	console.log("I have received a get request for the beer collection.");

		db.beer.find(function(err, beer) {
		if(beer) {
			res.json(beer);
		}
		else
		{
			res.json(err);
		}

	});

});

router.get('/beer/:id', function(req, res) {
	
	var id = mongojs.ObjectId(req.params.id);

	console.log("I have received a get request for the beer collection/:id.\n");

	db.beer.findOne({_id:id}, function(err, beer) {
		if(beer) {
			res.json(beer);
		}
		else
		{
			res.status(404);
			res.json(err);
		}

	});

});


router.get('/beer/name/:name', function(req, res) {
	console.log("I have received a get request for the beer collection/:name.\n");

	var name = req.params.name;

	console.log("I have received a get request for the beer collection/:name.");

	db.beer.findOne({name:name}, function(err, beer) {
		if(beer) {
			res.json(beer);
		}
		else
		{
			res.status(404);
			res.json(err);
		}

	});

});

// Get Routes, Wine

router.get('/wine', function(req, res) {
	console.log("I have received a get request for the wine collection.\n");

		db.wine.find(function(err, wine) {
		if(wine) {			
			res.json(wine);
		}
		else
		{
			res.json(err);
		}

	});

});

router.get('/wine/:id', function(req, res) {
	
	var id = mongojs.ObjectId(req.params.id);

	console.log("I have received a get request for the wine collection/:id.\n");

	db.wine.findOne({_id:id}, function(err, wine) {
		if(wine) {
			res.json(wine);
		}
		else
		{
			res.status(404);
			res.json(err);
		}

	});

});


router.get('/wine/name/:name', function(req, res) {
	
	var name = req.params.name;

	console.log("I have received a get request for the beer collection/:wine.\n");

	db.wine.findOne({name:name}, function(err, wine) {
		if(wine) {
			res.json(wine);
		}
		else
		{
			res.status(404);
			res.json(err);
		}

	});

});

// Get Routes, Spirits

router.get('/spirits', function(req, res) {
	console.log("I have received a get request for the spirits collection.\n");

		db.spirits.find(function(err, spirits) {
		if(spirits) {
			res.json(spirits);
		}
		else
		{
			res.json(err);
		}
	});

});

router.get('/spirits/:id', function(req, res) {
	console.log("I have received a get request for the spirits collection/:id.");

	var id = mongojs.ObjectId(req.params.id);

	db.spirits.findOne({_id:id}, function(err, spirits) {
		if(spirits) {
			res.json(spirits);
		}
		else
		{
			res.status(404);
			res.json(err);
		}

	});

});


router.get('/spirits/name/:name', function(req, res) {
	

	var name = req.params.name;

	console.log("I have received a get request for the spirits collection/:name.\n");

	db.spirits.findOne({name:name}, function(err, spirits) {
		if(spirits) {
			res.json(spirits);
		}
		else
		{
			res.status(404);
			res.json(err);
		}

	});

});

// Get Routes, Food

router.get('/food', function(req, res) {
	console.log("I have received a get request for the food collection.");

		db.food.find(function(err, food) {
		if(food) {
			res.json(food);
		}
		else
		{
			res.json(err);
		}
	});

});

router.get('/food/:id', function(req, res) {
	
	console.log("I have received a get request for the food collection/:id.\n");

	var id = mongojs.ObjectId(req.params.id);

	db.food.findOne({_id:id}, function(err, food) {
		if(food) {
			res.json(food);
		}
		else
		{
			res.status(404);
			res.json(err);
		}

	});

});


router.get('/food/name/:name', function(req, res) {
	console.log("I have received a get request for the food collection/:name.");

	var name = req.params.name;

	if(name==null)
	{
		console.log("Please put in a name...");
		return 1;

	}

	db.food.findOne({name:name}, function(err, food) {
		if(food) {
			res.json(food);
		}
		else
		{
			res.status(404);
			res.json(err);
		}

	});

});


// POST AND EDIT ROUTES -- Currently, requiring the content header to be application/json, but there were bugs that were 
// interfering with the recognition of 'application/json' when I was sending requests. For now, it is editted out until
// I can figure it out completely. I decided it was better to focus on the overall functionality at this point than a verification
// bell and whistle. 

// CURRENTLY, the posts are designed for free-form, rather than the schema, which will be integrated when I build a workable front-end
// that gives a form to fill out. So, have fun and go nuts for the time being! I have included some recommended fields, but they are by no
// means binding at the moment.


// POST WINE

	// EX FIELDS:
		// name, type, varietal, year/vintage, country, region, alccontent, flavor profile, comments, rare.
			// "name" : "Cotes du Rhone", "
			// type" : "Red Wine", 
			// "varietal" : "Guigal Rouge", 
			// "country" : "France", 
			// "region" : "Rhone", 
			// "alccontent" : "14.0",
			// "flavorprofile" : "Smooth and Supple", 
			// "comments" : "A flavorful, tannin-rich wine, spicey and richly palette. Can be racy if left on the shelf too long.",
			// "rare" : "false"

router.post('/wine', function(req, res) { 

	/* var content = req.header['content-type']

	if(content != 'application/json')
	{
		res.status(406);
		var s = "Unacceptable. API only supports application/json";
		res.send(s);
		console.log(s);
		return
	}*/

	console.log("I received a POST request for /wine");

	db.wine.insert(req.body, function(err, wine)
	{
		res.json(req.body);
	});

});

// POST BEER

	// EX FIELDS:
		// name, type, style (German, Belgian, Midwestern, Mexican, Chinese, etc.), brewery, country, flavor profile, comments.

			// "name" : "Warsteiner",
			// "type" : "Pilsner", 
			// "style" : "German-Rheinlander",
			// "brewery" : "Warstein Brauerei",
			// "country" : "Germany", 
			// "alccontent" : "4.8"
			// "flavorprofile" : "Light, refreshing, summery", 
			// "comments" : "A competent German beer, mass-produced, but falls under the Reinheitsgebot, so the flavor is mellow and pure."

router.post('/beer', function(req, res) { 

	/* var content = req.header['content-type']

	if(content != 'application/json')
	{
		res.status(406);
		var s = "Unacceptable. API only supports application/json";
		res.send(s);
		console.log(s);
		return
	}*/

	console.log("I received a POST request for /beer");

	db.beer.insert(req.body, function(err, beer)
	{
		res.json(req.body);
	});

});

// POST SPIRITS

	// EX FIELDS:
		// name, type, distillery, country, alccontent, flavor profile, comments.

			// "name" : "Smirnoff",
			// "type" : "Vodka", 
			// "distillery" : "Diageo",
			// "country" : "Russia",
			// "alccontent" : "40",
			// "flavorprofile" : "Spirited, rubbing alcohol", 
			// "comments" : "A mid-range vodka. Won't give you a hangover if drunk in moderation..."


router.post('/spirits', function(req, res) { 

	/* var content = req.header['content-type']

	if(content != 'application/json')
	{
		res.status(406);
		var s = "Unacceptable. API only supports application/json";
		res.send(s);
		console.log(s);
		return
	}*/

	console.log("I received a POST request for /spirits");

	db.spirits.insert(req.body, function(err, spirits)
	{
		res.json(req.body);
	});

});

// POST FOOD

	// example fields
		// name, type, comments

		// "name" : "Brie",
		// "type" : "Soft cheese",
		// "comments" : "Very versatile, buttery"

router.post('/food', function(req, res) { 

	/* var content = req.header['content-type']

	if(content != 'application/json')
	{
		res.status(406);
		var s = "Unacceptable. API only supports application/json";
		res.send(s);
		console.log(s);
		return
	}*/

	console.log("I received a POST request for /food");

	db.food.insert(req.body, function(err, food)
	{
		res.json(req.body);
	});

});


// Put / EDIT - currently is only set for certain edits. Meant to correct errors, not re-do entire items.

router.put('/wine/:id', function(req, res) {

	// worry about content control later.

	var id = mongojs.ObjectId(req.params.id);

	console.log("I recieved a PUT for the wine collection.");

	db.wine.findAndModify({
		query: {_id: id},
		update: {$set : {
			name: req.body.name,
			type: req.body.type,
			varietal: req.body.varietal,
			country: req.body.country,
			region: req.body.region,
			year: req.body.year,
			alccontent: req.body.alccontent,
			pairing: req.body.pairing,
			flavorprofile: req.body.flavorprofile,
			comments: req.body.comments,
			rare: req.body.rare,
			}}, new: true
		}, function(err, wine){res.json(wine);}

		);

});

router.put('/beer/:id', function(req, res) {

	// worry about content control later.

	var id = mongojs.ObjectId(req.params.id);

	console.log("I recieved a PUT for the beer collection.");

	db.beer.findAndModify({
		query: {_id: id},
		update: {$set : {
			name: req.body.name,
			type: req.body.type,
			brewery: req.body.brewery,
			style: req.body.style,
			country: req.body.style,
			alccontent: req.body.alccontent,
			pairing: req.body.pairing,
			flavorprofile: req.body.flavorprofile,
			comments: req.body.comments
			}}, new: true
		}, function(err, beer){res.json(beer);}

		);

});

router.put('/spirits/:id', function(req, res) {

	// worry about content control later.

	var id = mongojs.ObjectId(req.params.id);

	console.log("I recieved a PUT for the spirits collection.");

	db.spirits.findAndModify({
		query: {_id: id},
		update: {$set : {
			name: req.body.name,
			type: req.body.type,
			country: req.body.country,
			distillery: req.body.distillery,
			alccontent: req.body.alccontent,
			pairing: req.body.pairing,
			flavorprofile: req.body.flavorprofile,
			comments: req.body.comments
			}}, new: true
		}, function(err, spirits){res.json(spirits);}

		);

});

router.put('/food/:id', function(req, res) {

	// worry about content control later.

	var id = mongojs.ObjectId(req.params.id);

	console.log("I recieved a PUT for the food collection.");

	db.food.findAndModify({
		query: {_id: id},
		update: {$set : {
			name: req.body.name,
			type: req.body.type,
			comments: req.body.comments

			}}, new: true
		}, function(err, food){res.json(food);}

		);

});

// Add a pairing:

//POST for wine to add a food pairing
router.post('/wine/:id/:foodid', function(req,res){
		
	//set up id var using the param and convert
	var wine_id = mongojs.ObjectId(req.params.id); 
	var food_id = mongojs.ObjectId(req.params.foodid);
	
	//message console
	console.log("I received a POST for a /wine to add a /food");
	
	//insert the new data into the db from body
	db.wine.findAndModify({ 
		query: {_id: wine_id},
		update: { $addToSet: {
			pairing: food_id
		}},
		new: true
		}, function(err,wine){res.json(wine);}
	);

	db.food.findAndModify({
		query: {_id: wine_id},
		update: { $addToSet: {
			pairswith: food_id
		}},
		new: true
		}, function(err,food){/*res.json(food);*/}
	); 
});

//POST for beer to add a food pairing
router.post('/beer/:id/:foodid', function(req,res){
		
	//set up id var using the param and convert
	var beer_id = mongojs.ObjectId(req.params.id); 
	var food_id = mongojs.ObjectId(req.params.foodid);
	
	//message console
	console.log("I received a POST for a /beer to add a /food");
	
	db.beer.findOne({_id:beer_id}, function(err, beer){
		if(beer)
		{
	//insert the new data into the db from body
			db.beer.findAndModify({ 
				query: {_id: beer_id},
				update: { $addToSet: {
					pairing: food_id
				}},
				new: true
				}, function(err,beer){res.json(beer);}
			);

			db.food.findAndModify({
				query: {_id: food_id},
				update: { $addToSet: {
					pairswith: beer_id
				}},
				new: true
				}, function(err,food){/*res.json(food);*/}
			); 
		}
		else
		{
			res.status(404);
			res.json(beer);
		}
	});
});

//POST for spirits to add a food pairing
router.post('/spirits/:id/:foodid', function(req,res){
		
	//set up id var using the param and convert
	var spirits_id = mongojs.ObjectId(req.params.id); 
	var food_id = mongojs.ObjectId(req.params.foodid);
	
	//message console
	console.log("I received a POST for a /spirits to add a /food");
	
	//insert the new data into the db from body
	db.spirits.findOne({_id: spirits_id}, function(err, spirits){
		if(spirits)
		{
			db.spirits.findAndModify({ 
				query: {_id: spirits_id},
				update: { $addToSet: {
				pairing: food_id
			}},
			new: true
			}, function(err,spirits){res.json(spirits);}
			);

			db.food.findAndModify({
				query: {_id: food_id},
				update: { $addToSet: {
					pairswith: spirits_id
				}},
				new: true
				}, function(err,food){/*res.json(food);*/}
			); 
		}
		else	
		{
		res.status('404');
		res.json(spirits);

		}
	});
});

//POST for a food to add a drink pairing
router.post('/food/:id/:drinkid', function(req,res){
		
	//set up id var using the param and convert
	var food_id = mongojs.ObjectId(req.params.id);
	var drink_id = mongojs.ObjectId(req.params.drinkid); 
	
	//message console
	console.log("I received a POST for a /food pairing");
	db.food.findOne({_id: food_id}, function(err, finders){
		if(finders) 
		{
	//insert the new data into the db from body
			db.food.findAndModify({ 
				query: {_id: food_id},
				update: { $addToSet: {
					pairswith: drink_id
				}},
				new: true
				}, function(err,food){/*res.json(food);*/}
			); 

			db.wine.findOne({_id:drink_id}, function(err, wine) {
				if(wine)
				{
				db.wine.findAndModify({ 
					query: {_id: drink_id},
					update: { $addToSet: {
						pairing: food_id
					}},
					new: true
					}, function(err,wine){/*res.json(wine);*/}
					); 
				}
			});

			db.beer.findOne({_id:drink_id}, function(err, beer) {
				if(beer)
				{
				db.beer.findAndModify({ 
					query: {_id: drink_id},
					update: { $addToSet: {
						pairing: food_id
					}},
					new: true
					}, function(err,beer){/*res.json(beer);*/}
					); 
				}
			});

			db.spirits.findOne({_id:drink_id}, function(err, spirits) {
				if(spirits)
				{
				db.spirits.findAndModify({ 
					query: {_id: drink_id},
					update: { $addToSet: {
						pairing: food_id
					}},
					new: true
					}, function(err,spirits){/*res.json(spirits);*/}
					); 
				}
			});

			console.log("Transmission successful.");
			console.log(finders);
			res.json(finders);
		}
		else
		{
			res.status(404);
			res.json(finders);
		}
	});


});


// Delete Routes -- for most cases, please use :id only. Names are purely experimental / so I can remove large numbers of same
// named objects for testing. May be further integrated for final project/alpha release version of the app.

// WINE ROUTES - id

router.delete('/wine/:id', function(req, res){

	var wine_id = mongojs.ObjectId(req.params.id);

	console.log("I have received a DELETE request");

	db.wine.findOne({_id:wine_id}, function(err, wine) {
		if(wine)
		{
			db.wine.remove({_id:wine_id}, function(err, byewine)
			{
				db.food.find().forEach(function(err, food){
				if(food!=null) {
					db.food.findAndModify({
						query: {_id: food._id},
						update: {$pull :
							{ pairswith: wine_id 
							}},
							new: true,
					}, function(err, food2){}
					);
				}
				});
				res.json(byewine);
			});
		}

		else
		{
			res.status(404);
			res.json(wine);
		}


	});

});

// WINE ROUTES - name

router.delete('/wine/name/:name', function(req, res){

	var wine_name = req.params.name;

	console.log("I have received a DELETE request");

	db.wine.findOne({name:wine_name}, function(err, wine) {
		if(wine)
			{
			db.wine.remove({name:wine_name}, function(err, byewine)
				{
					db.food.find().forEach(function(err, food){
					if(food!=null) {
						db.food.findAndModify({
							query: {_id: food._id},
							update: {$pull :
								{ pairswith: wine._id 
								}},
								new: true,
						}, function(err, food2){}
						);
					}
				});
				res.json(byewine);
			});
		}
		else if(wine_name == null)	// in case no name is entered -- currently robustness doesn't work.
		{
			res.status(404);
			res.jason(wine);
		}

		else
		{
			res.status(404);
			res.json(wine);
		}

	});

});

// BEER ROUTES - ID

router.delete('/beer/:id', function(req, res){

	var beer_id = mongojs.ObjectId(req.params.id);

	console.log("I have received a DELETE request");

	db.beer.findOne({_id:beer_id}, function(err, beer) {
		if(beer)
		{
			db.beer.remove({_id:beer_id}, function(err, byebeer)
			{
				db.food.find().forEach(function(err, food){
				if(food!=null) {
					db.food.findAndModify({
						query: {_id: food._id},
						update: {$pull :
							{ pairswith: beer_id 
							}},
							new: true,
					}, function(err, food2){}
					);
				}
				});
				res.json(byebeer);
			});
		}
		else
		{
			res.status(404)
			res.json(beer);
		}

	});

});

// BEER ROUTES - NAME

router.delete('/beer/name/:name', function(req, res){

	var name = req.params.name;

	console.log("I have received a DELETE request");

	db.beer.findOne({name:name}, function(err, beer) {
		if(beer)
		{
		db.beer.remove({name:name}, function(err, byebeer)
			{
				db.food.find().forEach(function(err, food){
				if(food!=null) {
					db.food.findAndModify({
						query: {_id: food._id},
						update: {$pull :
							{ pairswith: beer._id 
							}},
							new: true,
					}, function(err, food2){}
					);
				}
				});
				res.json(byebeer);
			});
		}
		else
		{
			res.status(404)
			res.json(beer);
		}

	});

});

// SPIRITS ROUTE - ID

router.delete('/spirits/:id', function(req, res){

	var spirits_id = mongojs.ObjectId(req.params.id);

	console.log("I have received a DELETE request");

	db.spirits.findOne({_id:spirits_id}, function(err, spirits) {
		if(spirits)
		{
			db.spirits.remove({_id:spirits_id}, function(err, byespirits)
			{
				db.food.find().forEach(function(err, food){
				if(food!=null) {
					db.food.findAndModify({
						query: {_id: food._id},
						update: {$pull :
							{ pairswith: spirits_id 
							}},
							new: true,
					}, function(err, food2){}
					);
				}
				});
				res.json(byespirits);
			});
		}
		else
		{
			res.status(404)
			res.json(spirits);
		}

	});

});

// SPIRITS ROUTE - NAME

router.delete('/spirits/name/:name', function(req, res){

	var name = req.params.name;

	console.log("I have received a DELETE request");

	db.spirits.findOne({name:name}, function(err, spirits) {
		if(spirits)
		{
			db.spirits.remove({name:name}, function(err, byespirits)
			{
				db.food.find().forEach(function(err, food){
				if(food!=null) {
					db.food.findAndModify({
						query: {_id: food._id},
						update: {$pull :
							{ pairswith: spirits._id 
							}},
							new: true,
					}, function(err, food2){}
					);
				}
				});
			res.json(byespirits);
			});
		}
		else
		{
			res.status(404)
			res.json(spirits);
		}

	});

});

// FOOD ROUTE - ID

router.delete('/food/:id', function(req, res){ 

	var food_id = mongojs.ObjectId(req.params.id);

	db.food.findOne({_id:food_id}, function(err, food)
	{
		if(food)
		{
			console.log("I have recieved a DELETE request in FOOD.")
			db.food.remove({_id: food_id}, function(err, byefood){});
			db.wine.find().forEach(function(err, wine){
				if(wine!=null) {
					db.wine.findAndModify({
						query: {_id: wine._id},
						update: {$pull :
							{ pairing: food_id 
							}},
							new: true,
					}, function(err, food2){}
					);
				}
			});

			db.beer.find().forEach(function(err, beer){
				if(beer!=null) {
					db.beer.findAndModify({
						query: {_id: beer._id},
						update: {$pull :
							{ pairing: food_id 
							}},
							new: true,
					}, function(err, food2){}
					);
				}
			});

			db.spirits.find().forEach(function(err, spirits){
				if(spirits!=null) {
					db.spirits.findAndModify({
						query: {_id: spirits._id},
						update: {$pull :
							{ pairing: food_id 
							}},
							new: true,
					}, function(err, food2){}
					);
				}
			});

			res.json("FOOD REMOVED!");

		}

		else 
		{
			console.log("Food was not found");
			res.status(404);
			res.json(docs);
		}

	
	});


});

// FOOD ROUTE - NAME - not fully implemented yet, use at own risk.

router.delete('/food/name/:name', function(req, res){

	var food_name = req.params.name;

	console.log("I have received a DELETE request");

	db.food.findOne({name:food_name}, function(err, food) {
		if(food)
		{
			console.log("I have recieved a DELETE request in FOOD.")
			db.food.remove({name: food_name}, function(err, byefood){});
			db.wine.find().forEach(function(err, wine){
				if(wine!=null) {
					db.wine.findAndModify({
						query: {_id: wine._id},
						update: {$pull :
							{ pairing: food._id 
							}},
							new: true,
					}, function(err, food2){}
					);
				}
			});

			db.beer.find().forEach(function(err, beer){
				if(beer!=null) {
					db.wine.findAndModify({
						query: {_id: beer._id},
						update: {$pull :
							{ pairing: food._id 
							}},
							new: true,
					}, function(err, food2){}
					);
				}
			});

			db.spirits.find().forEach(function(err, spirits){
				if(spirits!=null) {
					db.spirits.findAndModify({
						query: {_id: spirits._id},
						update: {$pull :
							{ pairing: food._id 
							}},
							new: true,
					}, function(err, food2){}
					);
				}
			});

			res.json("FOOD REMOVED!");

		}

		else 
		{
			console.log("Food was not found");
			res.status(404);
			res.json(food);
		}


	});

});


// USER Get

router.get('/user', function(req, res) {
	console.log("I have received a get request for the user collection.");

		db.user.find(function(err, user) {
		if(user) {
			res.json(user);
		}
		else
		{
			res.json(err);
		}
	});

});

router.get('/user/:userName/wine/', function(req, res) {
	console.log("I have received a get request for the user " + _userName + " collection.");

		var _userName = req.params.userName

		db.wine.findOne({username:_userName}, function(err, name) {
			if(name)
			{
				db.wine.find(function(err, wine) {
					if(wine) {			
						res.json(wine);
					}
					else
					{
						res.json(err);
					}
				});
			}
			else
			{
				res.json(err);
			}
	});
			
});

router.get('/user/:userName/wine/:name', function(req, res) {
	

		var _userName = req.params.userName;
		var _wineName = req.params.name;

		console.log("I have received a get request for the user " + _userName + " collection for wine " + _wineName + ".");

		db.user.find(function(err, user) {
		if(user) {
			db.wine.findOne({username:_userName}, function(err, name) {
			   if(name) {
					db.wine.findOne({name:_wineName}, function(err, wine) {
						if(wine)
						{
							res.json(wine);
						}
						else
						{
							res.json(err);
						}
					});
				}
				else
				{
					res.json(err);
				}
			});
		}
	});

});

router.get('/user/:userName/wine/type/:type', function(req, res) {
	

		var _userName = req.params.userName;
		var _wineType = req.params.type;

		console.log("I have received a get request for the user " + _userName + " collection for winetype " + _wineType + ".");

		db.user.find(function(err, user) {
		if(user) {
			db.wine.findOne({username:_userName}, function(err, name) {
			   if(name) {
					db.wine.find({type:_wineType, username: _userName}, function(err, wine) {
						if(wine)
						{
							res.json(wine);
						}
						else
						{
							res.json(err);
						}
					});
				}
				else
				{
					res.json(err);
				}
			});
		}
	});

});

router.get('/user/:userName/wine/region/:region', function(req, res) {
	

		var _userName = req.params.userName;
		var _wineRegion = req.params.region;

		console.log("I have received a get request for the user " + _userName + " collection for region " + _wineRegion + ".");

		db.user.find(function(err, user) {
		if(user) {
			db.wine.findOne({username:_userName}, function(err, name) {
			   if(name) {
					db.wine.find({region:_wineRegion, username:_userName}, function(err, wine) {
						if(wine)
						{
							res.json(wine);
						}
						else
						{
							res.json(err);
						}
					});
				}
				else
				{
					res.json(err);
				}
			});
		}
	});

});

router.get('/user/:userName/wine/year/:year', function(req, res) {
	

		var _userName = req.params.userName;
		var _wineYear = req.params.year;

		console.log("I have received a get request for the user " + _userName + " collection for wine " + _wineName + ".");

		db.user.find(function(err, user) {
		if(user) {
			db.wine.findOne({username:_userName}, function(err, name) {
			   if(name) {
					db.wine.find({year:_wineYear, username:_userName}, function(err, wine) {
						if(wine)
						{
							res.json(wine);
						}
						else
						{
							res.json(err);
						}
					});
				}
				else
				{
					res.json(err);
				}
			});
		}
	});

});

// Get something with food. Gives general suggestions right now.

router.get('/user/:userName/food/wine/:type', function(req, res) {
	

		var _userName = req.params.userName;
		var _wineType = req.params.type;

		console.log("I have received a get request for the user " + _userName + " collection for wine " + _wineType + ".");

		db.user.find(function(err, user) {
		if(user) {
			db.wine.findOne({username:_userName, type:_wineType}, function(err, name) {
			   if(name) {
					db.food.find({winetype:_wineType}, function(err, wine) {
						if(wine)
						{
							res.json(wine);
						}
						else
						{
							res.json(err);
						}
					});
				}
				else
				{
					res.json(err);
				}
			});
		}
	});

});

// User Post

router.post('/user', function(req, res) { 

	/* var content = req.header['content-type']

	if(content != 'application/json')
	{
		res.status(406);
		var s = "Unacceptable. API only supports application/json";
		res.send(s);
		console.log(s);
		return
	}*/

	console.log("I received a POST request for /user");

	db.user.insert(req.body, function(err, user)
	{
		res.json(req.body);
	});

});

router.post('/user/:userName/wine/', function(req, res) { 

	_userName = req.params.userName;

	/* var content = req.header['content-type']

	if(content != 'application/json')
	{
		res.status(406);
		var s = "Unacceptable. API only supports application/json";
		res.send(s);
		console.log(s);
		return
	}*/

	console.log("I received a POST request for user");

	db.wine.insert(req.body, function(err, wine)
	{
		res.json(req.body);
	});

});

router.post('/user/:userName/food/', function(req, res) { 

	_userName = req.params.userName;

	/* var content = req.header['content-type']

	if(content != 'application/json')
	{
		res.status(406);
		var s = "Unacceptable. API only supports application/json";
		res.send(s);
		console.log(s);
		return
	}*/

	console.log("I received a POST request for user");

	db.food.insert(req.body, function(err, wine)
	{
		res.json(req.body);
	});

});

// PUT paths

// For wine only at the moment -- as I develop this into a full app,
// I will fully integrate the other aspects. This is based on the put/edit
// code above, but reduced for simplicity sake for the final project application

router.put('/user/:userName/wine/:name', function(req, res) {

	// worry about content control later.

	var _userName = req.params.userName;
	var _wineName = req.params.name;

	console.log("I recieved a PUT for " + _wineName + " in the wine collection of user: " + _userName + ".");


	db.wine.findAndModify({
		query: {name: _wineName},
		update: {$set : {
			name: req.body.name,
			type: req.body.type,
			region: req.body.region,
			year: req.body.year,
			username: _userName
			}}, new: true
		}, function(err, wine){res.json(wine);}

	);
});


// User Delete

router.delete('/user/:userName/wine/:name', function(req, res){

	var _userName = req.params.userName;
	var _wineName = req.params.name;

	console.log("I have received a DELETE request from user: " + _userName + " for " + _wineName + ".");

	db.wine.findOne({username:_userName, name:_wineName}, function(err, wine) {
		if(wine)
		{
			db.wine.remove({name:_wineName}, function(err, byewine)
			{
				res.json(byewine);
			});
		}
		else
		{
			res.json(wine);
		}

	});

});

router.delete('/user/:id', function(req, res){

	var user_id = mongojs.ObjectId(req.params.id);

	console.log(user_id)
	console.log("I have received a DELETE request");

	db.user.findOne({_id:user_id}, function(err, user) {
		if(user)
		{
			db.user.remove({_id:user_id}, function(err, byeuser)
			{
				db.user.find().forEach(function(err, user){
				if(user!=null) {
					db.user.findAndModify({
						query: {_id:user_id},
					}, function(err, user2){}	// kept just in case
					);
				}
				});
			res.json(byeuser);
			});
		}
		else
		{
			res.status(404)
			res.json(user);
		}

	});

});

router.delete('/user/name/:name', function(req, res){

	var name = req.params.name;

	console.log("I have received a DELETE request");

	db.user.findOne({username:name}, function(err, user) {
		if(user)
		{
			db.user.remove({username:name}, function(err, byeuser)
			{
				db.user.find().forEach(function(err, user){
				if(user!=null) {
					db.user.findAndModify({
						query: {_id: user._id},
					}, function(err, user2){}
					);
				}
				});
			res.json(byeuser);
			});
		}
		else
		{
			res.status(404)
			res.json(user);
		}

	});

});


module.exports = router;