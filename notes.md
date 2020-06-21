# Tables

*[Playground](https://www.db-fiddle.com/f/jwvD3CrkcGQpKpQwNhiJDf/5)*

##### Relationships
| a&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; | b |
|:-|:-|
| 1 | 3 |
| 3 | 4 |
| 2 | 5 |

##### Project
| id&nbsp;&nbsp;&nbsp; | tag | name |
|:-|:-|:-|
| 1 | Priority | Project One |
| 2 | Priority | Project Two |
| 3 | ChannelGroup | Sub Project One |
| 4 | Channel | Sub Sub Project One |
| 5 | Channel | Sub Project Two |


### Schema 

```sql
CREATE TABLE projects (
	id INTEGER PRIMARY KEY,
	tag VARCHAR(128) NOT NULL,
	name VARCHAR (128) NOT NULL
);

CREATE TABLE relationships (
	a INTEGER NOT NULL
  		REFERENCES projects(id)
  		ON UPDATE CASCADE
  		ON DELETE CASCADE,
	b INTEGER NOT NULL
  		REFERENCES projects(id)
  		ON UPDATE CASCADE
  		ON DELETE CASCADE,
	PRIMARY KEY (a, b)
);

CREATE INDEX a_idx
	ON relationships (a);
CREATE INDEX b_idx
	ON relationships (b);
CREATE UNIQUE INDEX pair_unique_idx
	ON relationships (LEAST(a, b), GREATEST(a, b));
ALTER TABLE relationships 
    ADD CONSTRAINT no_self_loops_chk CHECK (a <> b);
```

### Data

```sql
INSERT INTO projects (id, tag, name)
	VALUES (1, 'Priority', 'Project One');
INSERT INTO projects (id, tag, name)
	VALUES (2, 'Priority', 'Project Two');
INSERT INTO projects (id, tag, name)
	VALUES (3, 'ChannelGroup', 'Sub Project One');
INSERT INTO projects (id, tag, name)
	VALUES (4, 'Channel', 'Sub Sub Project One');
INSERT INTO projects (id, tag, name)
	VALUES (5, 'Channel', 'Sub Project Two');

INSERT INTO relationships (a, b)
	VALUES (1, 3), (3, 4), (2, 5);
```

### Queries
- **Transitive closure** (All paths from root)

```sql
WITH RECURSIVE transitive_closure (a, b, distance, path_string) AS
(SELECT a, b, 1 AS distance, '.' || a || '.' || b || '.' path_string
 FROM relationships 
 UNION ALL
 SELECT tc.a, r.b, tc.distance + 1, tc.path_string || r.b || '.' AS path_string
 FROM relationships AS r
 JOIN  transitive_closure AS tc
 ON r.a = tc.b
 WHERE tc.path_string NOT LIKE '%.' || r.b || '.%'
)
SELECT * FROM transitive_closure
WHERE transitive_closure.a = @root_id
ORDER BY a, b, distance;
```

Options for query

- Run two queries, one for selecting desired root, and another to select the related nodes
- Add an artificial root project to mark roots

Example with 'Project One'

Add a Root node 0 tagged 'Root' to projects
Add a relationship from 0 to 1 to mark 1 as a root node

```sql
INSERT INTO projects (id, tag, name)
	VALUES(0, 'Root', 'Root');
INSERT INTO relationships (a, b)
  VALUES (0, 1);
SELECT id, tag, name FROM transitive_closure AS tc
LEFT JOIN projects AS p
ON tc.b = p.id
WHERE tc.a = 1 OR (tc.a = 0 AND tc.b = 1 AND distance = 1)
ORDER BY a, b, distance;
```

