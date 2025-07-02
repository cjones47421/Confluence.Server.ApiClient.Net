# Confluence Query Language (CQL) Overview

Confluence Query Language (CQL) is a powerful query language provided by Atlassian Confluence to search for content using structured queries. CQL allows you to filter and find pages, blog posts, attachments, and other content types based on various fields and criteria.

## Basic Syntax
A CQL query consists of one or more field-operator-value expressions, optionally combined with logical operators (`AND`, `OR`, `NOT`).

Example:
```
type=page AND space=DEV AND title~"API"
```

## Common Fields
- `type`: The type of content (e.g., `page`, `blogpost`, `attachment`)
- `space`: The space key (e.g., `space=EAS`)
- `title`: The title of the content
- `text`: Full-text search within content
- `creator`: The user who created the content
- `created`: The date the content was created
- `lastmodified`: The date the content was last modified
- `label`: Content label

## Operators
- `=`: Equal to
- `!=`: Not equal to
- `IN`: Value is in a list
- `NOT IN`: Value is not in a list
- `~`: Contains (for text fields)
- `>` `<` `>=` `<=`: Greater/less than (for dates/numbers)

## Examples
- Find all pages in a space:
```
type=page AND space=DEV
```
- Find pages with a specific label:
```
type=page AND label=how-to
```
- Find content created by a user:
```
creator=jsmith
```
- Find pages created after a certain date:
```
type=page AND created>2023-01-01
```
- Search for text in content:
```
text~"release notes"
```

## Advanced Usage
- Combine multiple conditions:
```
type=page AND (space=DEV OR space=OPS)
```
- Negate a condition:
```
type=page AND NOT label=deprecated
```

## API Usage
To use CQL in the Confluence REST API, use the `/rest/api/content/search` endpoint with the `cql` query parameter:
```
GET /rest/api/content/search?cql=type=page AND space=DEV
```

## References
- [Atlassian CQL Documentation](https://developer.atlassian.com/cloud/confluence/advanced-searching-using-cql/)
- [Confluence REST API Reference](https://developer.atlassian.com/cloud/confluence/rest/api-group-search/#api-wiki-rest-api-search-get)

---

This document provides a quick reference for using CQL in your Confluence integrations and API queries.
