//db = db.getSiblingDB('BookNote');

//db.createCollection('Notes', {
//    validator: {
//        $jsonSchema: {
//            bsonType: 'object',
//            required: ['Id', 'PatId', 'Patient', 'Notes']
//            properties: {
//                Id: {
//                    bsonType: 'int',
//                    description: 'must be a int and is required'
//                },
//                PatId: {
//                    bsonType: 'string',
//                    description: 'must be a string and is required'
//                },
//                Patient: {
//                    bsonType: 'string',
//                    description: 'must be a string and is required'
//                },
//                Notes: {
//                    bsonType: 'string',
//                    description: 'must be a string and is required'
//                }
//            }
//        }
//    }
//});
