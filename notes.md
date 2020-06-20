## Sample Tables
### Project
| id&nbsp;&nbsp;&nbsp; | tag | name |
|:-|:-|:-|
| 1 | Priority | Project One |
| 2 | Priority | Project Two |
| 3 | ChannelGroup | Sub Project One |
| 4 | Channel | Sub Sub Project One |
| 5 | Channel | Sub Project Two |

### Relationships
| a&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; | b |
|:-|:-|
| 1 | 3 |
| 3 | 4 |
| 2 | 5 |

### Schema
[Playground](https://www.db-fiddle.com/f/jwvD3CrkcGQpKpQwNhiJDf/1)

```sql
CREATE TABLE projects (
	id INTEGER PRIMARY KEY,
	tag VARCHAR(128) NOT NULL,
	name VARCHAR (128) NOT NULL
);

CREATE TABLE relationships (
	a INTEGER NOT NULL REFERENCES projects(id) ON UPDATE CASCADE ON DELETE CASCADE,
	b INTEGER NOT NULL REFERENCES projects(id) ON UPDATE CASCADE ON DELETE CASCADE,
	PRIMARY KEY (a, b)
);

CREATE INDEX a_idx ON relationships (a);
CREATE INDEX b_idx ON relationships (b);

INSERT INTO projects (id, tag, name) VALUES (1, 'Priority', 'Project One');
INSERT INTO projects (id, tag, name) VALUES (2, 'Priority', 'Project Two');
INSERT INTO projects (id, tag, name) VALUES (3, 'ChannelGroup', 'Sub Project One');
INSERT INTO projects (id, tag, name) VALUES (4, 'Channel', 'Sub Sub Project One');
INSERT INTO projects (id, tag, name) VALUES (5, 'Channel', 'Sub Project Two');

```

### Queries

Q. How to query full project?
A. All paths from root to leaf

#### Transitive closure

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
WHERE transitive_closure.a = 1
ORDER BY a, b, distance;
```