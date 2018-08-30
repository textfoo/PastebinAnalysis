db.getCollection("tag_analysis").remove({});

db.getCollection("tag_analysis").insert({
    'tag': 'crypto',
    'action': 'contains',
    'keywords': ['bitcoin']
});

db.getCollection("tag_analysis").insert({
    'tag': 'dox',
    'action': 'contains',
    'keywords': ["dox", "d0x", "doxx", "d0xx", "doxing", "d0xing", "|)ox", "|)0]["]
});


db.getCollection("tag_analysis").insert({
    'tag': 'code',
    'action': 'contains',
    'keywords': ["{", "};", ");", "?php"]
});

db.getCollection("tag_analysis").insert({
    'tag': 'url',
    'action': 'contains',
    'keywords': [".com", ".org", ".net", ".int", ".edu", ".gov", ".mil", ".link", "https://", "http://"]
});

