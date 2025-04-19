### 🔑 **Project Content Breakdown**

---

### 1. **`Question` Model**

The `Question` class represents a **domain model** in your project and is used for interacting with the database.

**Explanation of the model:**
- `QuestionId`: A unique identifier for each question, marked with `[Key]` to signify it's the primary key.
- `UserId`: The ID of the user who created the question.
- `Title`: The title of the question (required field).
- `Description`: A detailed description of the question (required field).
- `CreatedDate`: The date and time when the question was created. This is set to `DateTime.UtcNow` when a new question is created.
- `Tags`: A string used to categorize or label the question (defaulted to "Common").

#### **Entity Framework Core**
- **DbContext**: Entity Framework Core uses `DbContext` to manage and interact with your database. In your project, the `QuestAppDbContext` class represents the database context, allowing you to perform operations like querying, adding, updating, and deleting questions.
  
---

### 2. **`QuestionController`**

The `QuestionController` is an **API Controller** that handles HTTP requests related to the `Question` model. It's responsible for processing requests (e.g., GET, POST, PUT, DELETE) and interacting with the `QuestAppDbContext` to perform database operations.

Here's a brief explanation of each part of the controller:

#### **Get All Questions (`GET /api/question`)**
```csharp
public IActionResult GetAll()
{
    var questionsDomain = dbContext.questions.ToList();
    if(!questionsDomain.Any())
    {
        return NotFound("No Question found");
    }
    return Ok(questionsDomain);
}
```
- **Purpose**: Retrieves all questions from the database.
- **Returns**:
  - `200 OK` with a list of questions if questions exist.
  - `404 Not Found` if no questions are available.

#### **Get Question by ID (`GET /api/question/{id:guid}`)**
```csharp
public IActionResult GetById([FromRoute] Guid id)
{
    var question = dbContext.questions.FirstOrDefault(q => q.QuestionId == id);
    if (question == null)
    {
        return NotFound("Question Not Found");
    }
    return Ok(question);
}
```
- **Purpose**: Retrieves a single question based on the `QuestionId` (GUID).
- **Returns**:
  - `200 OK` with the found question.
  - `404 Not Found` if the question doesn't exist.

#### **Create a New Question (`POST /api/question`)**
```csharp
public IActionResult Post([FromBody] Question question)
{
    if (question == null)
    {
        return BadRequest("Invalid Request Data");
    }

    question.CreatedDate = DateTime.UtcNow;
    dbContext.questions.Add(question);
    dbContext.SaveChanges();

    return CreatedAtAction(nameof(GetAll), new { id = question.QuestionId }, question);
}
```
- **Purpose**: Creates a new question by accepting a `Question` object in the request body, and then saving it to the database.
- **Returns**:
  - `201 Created` with the created question and location of the new resource.

#### **Update an Existing Question (`PUT /api/question/{id:guid}`)**
```csharp
public IActionResult Update([FromRoute] Guid id, [FromBody] Question question)
{
    if (question == null || id != question.QuestionId)
    {
        return BadRequest("Invalid or missing data");
    }

    var ExistingQuestion = dbContext.questions.FirstOrDefault(q => q.QuestionId == id);

    if (ExistingQuestion == null)
    {
        return NotFound("Question not found");
    }

    ExistingQuestion.Title = question.Title;
    ExistingQuestion.Description = question.Description;
    ExistingQuestion.Tags = question.Tags;

    dbContext.SaveChanges();
    return NoContent();
}
```
- **Purpose**: Updates an existing question, based on its `QuestionId`. It checks if the question exists and updates the necessary fields.
- **Returns**:
  - `204 No Content` when the update is successful.
  - `400 BadRequest` if there's invalid data.
  - `404 NotFound` if the question doesn’t exist.

#### **Delete a Question (`DELETE /api/question/{id:guid}`)**
```csharp
public IActionResult Delete([FromRoute] Guid id)
{
    var ExistingQuestion = dbContext.questions.FirstOrDefault(q => q.QuestionId == id);
    if (ExistingQuestion == null)
    {
        return NotFound("Question not found");
    }
    dbContext.questions.Remove(ExistingQuestion);
    dbContext.SaveChanges();
    return NoContent();
}
```
- **Purpose**: Deletes a question from the database based on the `QuestionId`.
- **Returns**:
  - `204 No Content` if deletion is successful.
  - `404 NotFound` if the question doesn't exist.

---

### 3. **Entity Framework and DbContext**

- **Entity Framework Core**: This is an **ORM (Object-Relational Mapper)** that maps your `Question` C# object to a table in the database.
- **DbContext**: `QuestAppDbContext` is the class that represents the session with the database. It manages the entities (models like `Question`) and performs operations like add, update, delete, and query.
  
In your project, `dbContext.questions` is the `DbSet<Question>`, which is a collection of `Question` entities that corresponds to the `Questions` table in the database. Entity Framework translates your LINQ queries (`dbContext.questions.ToList()`, `dbContext.questions.Add(question)`) into SQL queries that interact with the actual database.

---

### 4. **Database Interaction Flow**

- **When you call `dbContext.questions.ToList()`**: Entity Framework generates an SQL `SELECT * FROM Questions` to retrieve all questions from the database.
- **When you call `dbContext.questions.Add(question)`**: Entity Framework generates an SQL `INSERT INTO Questions (columns) VALUES (values)` to insert a new question.
- **When you call `dbContext.SaveChanges()`**: Entity Framework applies any changes made in the context (inserting, updating, or deleting records) to the actual database.

---

### Conclusion

This Web API allows for efficient management of `Question` entities using **CRUD operations**. It leverages **Entity Framework Core** to easily interact with the underlying SQL Server database, making it possible to quickly add, update, retrieve, and delete questions through API endpoints.
