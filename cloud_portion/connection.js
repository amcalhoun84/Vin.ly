var AWS = require('aws-sdk');

AWS.config.update({
	accessKeyId: "AKIAJIXWKXNI36LLEV2A",
	secretAccessKey: "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",
	region: "us-west-2",
	endpoint: "https://dynamodb.us-west-2.amazonaws.com"
});

var dynamodb = new AWS.DynamoDB();

var params = {
	TableName : "HW2Test",
	KeySchema: [
			{ AttributeName: "name", KeyType: "HASH"},
			{ AttributeName: "year", KeyType: "RANGE"}

		],
		AttributeDefinitions: [
			{ AttributeName: "name", AttributeType: "S"},
			{ AttributeName: "year", AttributeType: "N"}
		],
		ProvisionedThroughput: {       
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
