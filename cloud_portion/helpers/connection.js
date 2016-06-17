var AWS = require('aws-sdk');

AWS.config.update({
	region: "us-west-2",
	endpoint: "https://dynamodb.us-west-2.amazonaws.com"
});

var dynamodb = new AWS.DynamoDB();

var params = {
	TableName : "HW2Test",
		 KeySchema: [
			{ AttributeName: "name", KeyType: "HASH"},
			{ AttributeName: "type", KeyType: "RANGE"},
			{ AttributeName: "variety", KeyType: "HASH"},
			{ AttributeName: "year", KeyType: "HASH"},
			{ AttributeName: "description", KeyType: "HASH"}

		],
		AttributeDefinitions: [
			{ AttributeName: "name", AttributeType: "S"},
			{ AttributeName: "type", AttributeType: "S"},
			{ AttributeName: "variety", KeyType: "S"},
			{ AttributeName: "year", KeyType: "N"},
			{ AttributeName: "description", KeyType: "S"}

		],
		ProvisionedThoroughput: {
			ReadCapacityUnits: 5,
			WriteCapacityUnits: 5
		}
};

dynamodb.createTable(params, function(err, data) {
	if(err) {
		console.error("Unable to create the table. Error JSON:", JSON.stringify(err, null, 2));
	}
	else {
		console.log("Created Table. Table Description JSON:", JSON.stringify(data, null, 2));
	}
});
