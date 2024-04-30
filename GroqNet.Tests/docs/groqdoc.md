# Get started 

## Quickstart
Get up and running with the Groq API in a few minutes.

### Create an API Key
Please visit here to create an API Key.

### Set up your API Key (recommended)
Configure your API key as an environment variable. This approach streamlines your API usage by eliminating the need to include your API key in each request. Moreover, it enhances security by minimizing the risk of inadvertently including your API key in your codebase.


In your terminal of choice:

```
export GROQ_API_KEY=<your-api-key-here>
```

### Requesting your first chat completion

Install the Groq JavaScript library:

```
npm install --save groq-sdk
```

Performing a Chat Completion:

```JavaScript
"use strict";
const Groq = require("groq-sdk");
const groq = new Groq({
    apiKey: process.env.GROQ_API_KEY
});
async function main() {
    const chatCompletion = await getGroqChatCompletion();
    // Print the completion returned by the LLM.
    process.stdout.write(chatCompletion.choices[0]?.message?.content || "");
}
async function getGroqChatCompletion() {
    return groq.chat.completions.create({
        messages: [
            {
                role: "user",
                content: "Explain the importance of fast language models"
            }
        ],
        model: "mixtral-8x7b-32768"
    });
}
module.exports = {
    main,
    getGroqChatCompletion
};
```

Now that you have successfully received a chat completion, you can try out the other endpoints in the API.

### Next Steps
Check out the Playground to try out the Groq API in your browser
Join our GroqCloud developer community on Discord
Chat with our Docs at lightning speed using the Groq API!
Add a how-to on your project to the Groq API Cookbook

## Supported Models
GroqCloud currently supports the following models:

### LLaMA2 70b
Model ID: llama2-70b-4096
Developer: Meta
Context Window: 4,096 tokens
Model Card: https://huggingface.co/meta-llama/Llama-2-70b

### Mixtral 8x7b
Model ID: mixtral-8x7b-32768
Developer: Mistral
Context Window: 32,768 tokens
Model Card: https://huggingface.co/mistralai/Mixtral-8x7B-Instruct-v0.1

### Gemma 7b
Model ID: gemma-7b-it
Developer: Google
Context Window: 8,192 tokens
Model Card: https://huggingface.co/google/gemma-1.1-7b-it

These are chat type models and are directly accessible through the GroqCloud Models API endpoint using the model IDs mentioned above.

## API Error Codes and Responses
Our API uses standard HTTP response status codes to indicate the success or failure of an API request. In cases of errors, the body of the response will contain a JSON object with details about the error. Below are the error codes you may encounter, along with their descriptions and example response bodies.

### Error Codes Documentation
Our API uses specific error codes to indicate the success or failure of an API request. Understanding these codes and their implications is essential for effective error handling and debugging.

### Success Codes
- 200 OK: The request was successfully executed. No further action is needed.

### Client Error Codes
- 400 Bad Request: The server could not understand the request due to invalid syntax. Review the request format and ensure it is correct.
- 404 Not Found: The requested resource could not be found. Check the request URL and the existence of the resource.
- 422 Unprocessable Entity: The request was well-formed but could not be followed due to semantic errors. Verify the data provided for correctness and completeness.
- 429 Too Many Requests: Too many requests were sent in a given timeframe. Implement request throttling and respect rate limits.

### Server Error Codes
- 500 Internal Server Error: A generic error occurred on the server. Try the request again later or contact support if the issue persists.
- 502 Bad Gateway: The server received an invalid response from an upstream server. This may be a temporary issue; retrying the request might resolve it.
- 503 Service Unavailable: The server is not ready to handle the request, often due to maintenance or overload. Wait before retrying the request.

### Informational Codes
- 206 Partial Content: Only part of the resource is being delivered, usually in response to range headers sent by the client. Ensure this is expected for the request being made.

### Error Object Explanation
When an error occurs, our API returns a structured error object containing detailed information about the issue. This section explains the components of the error object to aid in troubleshooting and error handling.

### Error Object Structure
The error object follows a specific structure, providing a clear and actionable message alongside an error type classification:

```json
{
  "error": {
    "message": "String - description of the specific error",
    "type": "invalid_request_error"
  }
}
```

### Components
- error (object): The primary container for error details.
  - message (string): A descriptive message explaining the nature of the error, intended to aid developers in diagnosing the problem.
  - type (string): A classification of the error type, such as "invalid_request_error", indicating the general category of the problem encountered.

# Features

## Chat Completion Models
The Groq Chat Completions API processes a series of messages and generates output responses. These models can perform multi-turn discussions or tasks that require only one interaction.

### Required Parameters
- model: The language model which will perform the completion. See the models to learn more about available models.
- messages: A list of messages in the conversation so far. Each message is an object that has the following fields:
  - role:
    - system: sets the behavior of the assistant and can be used to provide specific instructions for how it should behave throughout the conversation.
    - user: Messages written by a user of the LLM.
    - assistant: Messages written by the LLM in a previous completion.
    - other message types are not currently supported
  - content: The text of a message.
  - name: An optional name to disambiguate messages from different users with the same role.
  - seed: Seed used for sampling. Groq attempts to return the same response to the same request with an identical seed.

### Optional Parameters
- temperature: Controls randomness of responses. A lower temperature leads to more predictable outputs while a higher temperature results in more varies and sometimes more creative outputs.
- max_tokens: The maximum number of tokens that the model can process in a single response. This limits ensures computational efficiency and resource management.
- top_p: A method of text generation where a model will only consider the most probable next tokens that make up the probability p. 0.5 means half of all likelihood-weighted options are considered.
- stream: User server-side events to send the completion in small deltas rather than in a single batch after all processing has finished. This reduces the time to first token received.
- stop: A stop sequence is a predefined or user-specified text string that signals an AI to stop generating content, ensuring its responses remain focused and concise.

### JSON mode (beta)
JSON mode is a beta feature that guarantees all chat completions are valid JSON.

Usage:

1. Set "response_format": {"type": "json_object"} in your chat completion request
1. Add a description of the desired JSON structure within the system prompt (see below for example system prompts)

Recommendations for best beta results:

- Mixtral performs best at generating JSON, followed by Gemma, then Llama
- Use pretty-printed JSON instead of compact JSON
- Keep prompts concise

Beta Limitations:

- Does not support streaming
- Does not support stop sequences

Error Code:

- Groq will return a 400 error with an error code of json_validate_failed if JSON generation fails.

Example system prompts:

```
You are a legal advisor who summarizes documents in JSON
```

```
You are a data analyst API capable of sentiment analysis that responds in JSON.  The JSON schema should include
{
  "sentiment_analysis": {
    "sentiment": "string (positive, negative, neutral)",
    "confidence_score": "number (0-1)"
    # Include additional fields as required
  }
}
```

### Generating Chat Completions with groq SDK

```
npm install --save groq-sdk
```

### Performing a basic Chat Completion

```JavaScript
"use strict";
const Groq = require("groq-sdk");
const groq = new Groq();
async function main() {
    const chatCompletion = await getGroqChatCompletion();
    // Print the completion returned by the LLM.
    process.stdout.write(chatCompletion.choices[0]?.message?.content || "");
}
const getGroqChatCompletion = async ()=>{
    return groq.chat.completions.create({
        //
        // Required parameters
        //
        messages: [
            // Set an optional system message. This sets the behavior of the
            // assistant and can be used to provide specific instructions for
            // how it should behave throughout the conversation.
            {
                role: "system",
                content: "you are a helpful assistant."
            },
            // Set a user message for the assistant to respond to.
            {
                role: "user",
                content: "Explain the importance of fast language models"
            }
        ],
        // The language model which will generate the completion.
        model: "mixtral-8x7b-32768",
        //
        // Optional parameters
        //
        // Controls randomness: lowering results in less random completions.
        // As the temperature approaches zero, the model will become deterministic
        // and repetitive.
        temperature: 0.5,
        // The maximum number of tokens to generate. Requests can use up to
        // 2048 tokens shared between prompt and completion.
        max_tokens: 1024,
        // Controls diversity via nucleus sampling: 0.5 means half of all
        // likelihood-weighted options are considered.
        top_p: 1,
        // A stop sequence is a predefined or user-specified text string that
        // signals an AI to stop generating content, ensuring its responses
        // remain focused and concise. Examples include punctuation marks and
        // markers like "[end]".
        stop: null,
        // If set, partial message deltas will be sent.
        stream: false
    });
};
module.exports = {
    main,
    getGroqChatCompletion
};
```

### Streaming a Chat Completion
To stream a completion, simply set the parameter stream=True. Then the completion function will return an iterator of completion deltas rather than a single, full completion.

```JavaScript
from groq import Groq

client = Groq()

stream = client.chat.completions.create(
    #
    # Required parameters
    #
    messages=[
        # Set an optional system message. This sets the behavior of the
        # assistant and can be used to provide specific instructions for
        # how it should behave throughout the conversation.
        {
            "role": "system",
            "content": "you are a helpful assistant."
        },
        # Set a user message for the assistant to respond to.
        {
            "role": "user",
            "content": "Explain the importance of fast language models",
        }
    ],

    # The language model which will generate the completion.
    model="mixtral-8x7b-32768",

    #
    # Optional parameters
    #

    # Controls randomness: lowering results in less random completions.
    # As the temperature approaches zero, the model will become deterministic
    # and repetitive.
    temperature=0.5,

    # The maximum number of tokens to generate. Requests can use up to
    # 2048 tokens shared between prompt and completion.
    max_tokens=1024,

    # Controls diversity via nucleus sampling: 0.5 means half of all
    # likelihood-weighted options are considered.
    top_p=1,

    # A stop sequence is a predefined or user-specified text string that
    # signals an AI to stop generating content, ensuring its responses
    # remain focused and concise. Examples include punctuation marks and
    # markers like "[end]".
    stop=None,

    # If set, partial message deltas will be sent.
    stream=True,
)

# Print the incremental deltas returned by the LLM.
for chunk in stream:
    print(chunk.choices[0].delta.content, end="")
```

### Streaming a chat completion with a stop sequence

```JavaScript
"use strict";
const Groq = require("groq-sdk");
const groq = new Groq();
async function main() {
    const stream = await getGroqChatStream();
    for await (const chunk of stream){
        // Print the completion returned by the LLM.
        process.stdout.write(chunk.choices[0]?.delta?.content || "");
    }
}
async function getGroqChatStream() {
    return groq.chat.completions.create({
        //
        // Required parameters
        //
        messages: [
            // Set an optional system message. This sets the behavior of the
            // assistant and can be used to provide specific instructions for
            // how it should behave throughout the conversation.
            {
                role: "system",
                content: "you are a helpful assistant."
            },
            // Set a user message for the assistant to respond to.
            {
                role: "user",
                content: "Start at 1 and count to 10.  Separate each number with a comma and a space"
            }
        ],
        // The language model which will generate the completion.
        model: "mixtral-8x7b-32768",
        //
        // Optional parameters
        //
        // Controls randomness: lowering results in less random completions.
        // As the temperature approaches zero, the model will become deterministic
        // and repetitive.
        temperature: 0.5,
        // The maximum number of tokens to generate. Requests can use up to
        // 2048 tokens shared between prompt and completion.
        max_tokens: 1024,
        // Controls diversity via nucleus sampling: 0.5 means half of all
        // likelihood-weighted options are considered.
        top_p: 1,
        // A stop sequence is a predefined or user-specified text string that
        // signals an AI to stop generating content, ensuring its responses
        // remain focused and concise. Examples include punctuation marks and
        // markers like "[end]".
        //
        // For this example, we will use ", 6" so that the llm stops counting at 5.
        // If multiple stop values are needed, an array of string may be passed,
        // stop: [", 6", ", six", ", Six"]
        stop: ", 6",
        // If set, partial message deltas will be sent.
        stream: true
    });
}
module.exports = {
    main,
    getGroqChatStream
};
```

### JSON Mode

```JavaScript
"use strict";
const Groq = require("groq-sdk");
const groq = new Groq();
const schema = {
    $defs: {
        Ingredient: {
            properties: {
                name: {
                    title: "Name",
                    type: "string"
                },
                quantity: {
                    title: "Quantity",
                    type: "string"
                },
                quantity_unit: {
                    anyOf: [
                        {
                            type: "string"
                        },
                        {
                            type: "null"
                        }
                    ],
                    title: "Quantity Unit"
                }
            },
            required: [
                "name",
                "quantity",
                "quantity_unit"
            ],
            title: "Ingredient",
            type: "object"
        }
    },
    properties: {
        recipe_name: {
            title: "Recipe Name",
            type: "string"
        },
        ingredients: {
            items: {
                $ref: "#/$defs/Ingredient"
            },
            title: "Ingredients",
            type: "array"
        },
        directions: {
            items: {
                type: "string"
            },
            title: "Directions",
            type: "array"
        }
    },
    required: [
        "recipe_name",
        "ingredients",
        "directions"
    ],
    title: "Recipe",
    type: "object"
};
class Ingredient {
    constructor(name, quantity, quantity_unit){
        this.name = name;
        this.quantity = quantity;
        this.quantity_unit = quantity_unit || null;
    }
}
class Recipe {
    constructor(recipe_name, ingredients, directions){
        this.recipe_name = recipe_name;
        this.ingredients = ingredients;
        this.directions = directions;
    }
}
async function getRecipe(recipe_name) {
    // Pretty printing improves completion results.
    jsonSchema = JSON.stringify(schema, null, 4);
    const chat_completion = await groq.chat.completions.create({
        messages: [
            {
                role: "system",
                content: `You are a recipe database that outputs recipes in JSON.\n'The JSON object must use the schema: ${jsonSchema}`
            },
            {
                role: "user",
                content: `Fetch a recipe for ${recipe_name}`
            }
        ],
        model: "mixtral-8x7b-32768",
        temperature: 0,
        stream: false,
        response_format: {
            type: "json_object"
        }
    });
    return Object.assign(new Recipe(), JSON.parse(chat_completion.choices[0].message.content));
}
function printRecipe(recipe) {
    console.log("Recipe:", recipe.recipe_name);
    console.log();
    console.log("Ingredients:");
    recipe.ingredients.forEach((ingredient)=>{
        console.log(`- ${ingredient.name}: ${ingredient.quantity} ${ingredient.quantity_unit || ""}`);
    });
    console.log();
    console.log("Directions:");
    recipe.directions.forEach((direction, step)=>{
        console.log(`${step + 1}. ${direction}`);
    });
}
async function main() {
    const recipe = await getRecipe("apple pie");
    printRecipe(recipe);
}
module.exports = {
    getRecipe,
    main
};
```

## OpenAI Compatibility
Groq's APIs are designed to be compatible with OpenAI's, with the goal of making it easy to leverage Groq in applications you may have already built. However, there are some nuanced differences where support is not yet available.

### Text Completion
The following fields are not supported and will result in a 400 error if they are supplied:

logprobs
logit_bias
top_logprobs


If N is supplied, it must be equal to 1.

### Temperature
If you set a temperature value of 0, it will be converted to `1e-8`. If you run into any issues, please try setting the value to a float32 `>` 0 and `<=` 2.

### URL
The base_url is https://api.groq.com/openai/v1

## Using Tools
Groq API endpoints support tool use for programmatic execution of specified operations through requests with explicitly defined operations. With tool use, Groq API model endpoints deliver structured JSON output that can be used to directly invoke functions from desired codebases.


### Models
The three models powered by Groq (llama2-70b, mixtral-8x7b and gemma-7b-it) all have support for tool use.

### Use Cases
- Convert natural language into API calls: Interpreting user queries in natural language, such as “What’s the weather in Palo Alto today?”, and translating them into specific API requests to fetch the requested information.
- Call external API: Automating the process of periodically gathering stock prices by calling an API, comparing these prices with predefined thresholds and automatically sending alerts when these thresholds are met.
- Resume parsing for recruitment: Analyzing resumes in natural language to extract structured data such as candidate name, skillsets, work history, and education, that can be used to populate a database of candidates matching certain criteria.

### Example
```python
from groq import Groq
import os
import json

client = Groq(api_key = os.getenv('GROQ_API_KEY'))
MODEL = 'mixtral-8x7b-32768'


# Example dummy function hard coded to return the score of an NBA game
def get_game_score(team_name):
    """Get the current score for a given NBA game"""
    if "warriors" in team_name.lower():
        return json.dumps({"game_id": "401585601", "status": 'Final', "home_team": "Los Angeles Lakers", "home_team_score": 121, "away_team": "Golden State Warriors", "away_team_score": 128})
    elif "lakers" in team_name.lower():
        return json.dumps({"game_id": "401585601", "status": 'Final', "home_team": "Los Angeles Lakers", "home_team_score": 121, "away_team": "Golden State Warriors", "away_team_score": 128})
    elif "nuggets" in team_name.lower():
        return json.dumps({"game_id": "401585577", "status": 'Final', "home_team": "Miami Heat", "home_team_score": 88, "away_team": "Denver Nuggets", "away_team_score": 100})
    elif "heat" in team_name.lower():
        return json.dumps({"game_id": "401585577", "status": 'Final', "home_team": "Miami Heat", "home_team_score": 88, "away_team": "Denver Nuggets", "away_team_score": 100})
    else:
        return json.dumps({"team_name": team_name, "score": "unknown"})

def run_conversation(user_prompt):
    # Step 1: send the conversation and available functions to the model
    messages=[
        {
            "role": "system",
            "content": "You are a function calling LLM that uses the data extracted from the get_game_score function to answer questions around NBA game scores. Include the team and their opponent in your response."
        },
        {
            "role": "user",
            "content": user_prompt,
        }
    ]
    tools = [
        {
            "type": "function",
            "function": {
                "name": "get_game_score",
                "description": "Get the score for a given NBA game",
                "parameters": {
                    "type": "object",
                    "properties": {
                        "team_name": {
                            "type": "string",
                            "description": "The name of the NBA team (e.g. 'Golden State Warriors')",
                        }
                    },
                    "required": ["team_name"],
                },
            },
        }
    ]
    response = client.chat.completions.create(
        model=MODEL,
        messages=messages,
        tools=tools,
        tool_choice="auto",  
        max_tokens=4096
    )

    response_message = response.choices[0].message
    tool_calls = response_message.tool_calls
    # Step 2: check if the model wanted to call a function
    if tool_calls:
        # Step 3: call the function
        # Note: the JSON response may not always be valid; be sure to handle errors
        available_functions = {
            "get_game_score": get_game_score,
        }  # only one function in this example, but you can have multiple
        messages.append(response_message)  # extend conversation with assistant's reply
        # Step 4: send the info for each function call and function response to the model
        for tool_call in tool_calls:
            function_name = tool_call.function.name
            function_to_call = available_functions[function_name]
            function_args = json.loads(tool_call.function.arguments)
            function_response = function_to_call(
                team_name=function_args.get("team_name")
            )
            messages.append(
                {
                    "tool_call_id": tool_call.id,
                    "role": "tool",
                    "name": function_name,
                    "content": function_response,
                }
            )  # extend conversation with function response
        second_response = client.chat.completions.create(
            model=MODEL,
            messages=messages
        )  # get a new response from the model where it can see the function response
        return second_response.choices[0].message.content
    
user_prompt = "What was the score of the Warriors game?"
print(run_conversation(user_prompt))
```

### Sequence of Steps
1. Initialize the API client: Set up the Groq Python client with your API key and specify the model to be used for generating conversational responses.
1. Define the function and conversation parameters: Create a user query and define a function (get_current_score) that can be called by the model, detailing its purpose, input parameters, and expected output format.
1. Process the model’s request: Submit the initial conversation to the model, and if the model requests to call the defined function, extract the necessary parameters from the model’s request and execute the function to get the response.
1. Incorporate function response into conversation: Append the function’s output to the conversation and a structured message and resubmit to the model, allowing it to generate a response that includes or reacts to the information provided by the function call.

### Tools Specifications
- tools: an array with each element representing a tool
  - type: a string indicating the category of the tool
  - function: an object that includes:
    - description - a string that describes the function’s purpose, guiding the model on when and how to use it
    - name: a string serving as the function’s identifier
    - parameters: an object that defines the parameters the function accepts

### Tool Choice
- tool_choice: A parameter that dictates if the model can invoke functions.
  - auto: The default setting where the model decides between sending a text response or calling a function
  - none: Equivalent to not providing any tool specification; the model won't call any functions

- Specifying a Function:
  - To mandate a specific function call, use {"type": "function", "function": {"name":"get_financial_data"}}
  - The model is constrained to utilize the function named

# Integrations

## Groq client libraries
Groq provides both a Python and JavaScript/Typescript client library.


### Groq JavaScript API Library
The Groq JavaScript library provides convenient access to the Groq REST API from server-side TypeScript or JavaScript.

The library includes type definitions for all request params and response fields, and offers both synchronous and asynchronous clients.

Installation
```
npm install --save groq-sdk
```

### Usage

Use the library and your secret key to run:

```JavaScript
"use strict";
const Groq = require("groq-sdk");
const groq = new Groq({
    apiKey: process.env.GROQ_API_KEY
});
async function main() {
    const chatCompletion = await getGroqChatCompletion();
    // Print the completion returned by the LLM.
    process.stdout.write(chatCompletion.choices[0]?.message?.content || "");
}
async function getGroqChatCompletion() {
    return groq.chat.completions.create({
        messages: [
            {
                role: "user",
                content: "Explain the importance of fast language models"
            }
        ],
        model: "mixtral-8x7b-32768"
    });
}
module.exports = {
    main,
    getGroqChatCompletion
};
```

The following response is generated:

```json
{
  "id": "34a9110d-c39d-423b-9ab9-9c748747b204",
  "object": "chat.completion",
  "created": 1708045122,
  "model": "mixtral-8x7b-32768",
  "system_fingerprint": "fp_dbffcd8265",
  "choices": [
    {
      "index": 0,
      "message": {
        "role": "assistant",
        "content": "Low latency Large Language Models (LLMs) are important in the field of artificial intelligence and natural language processing (NLP) for several reasons:\n\n1. Real-time applications: Low latency LLMs are essential for real-time applications such as chatbots, voice assistants, and real-time translation services. These applications require immediate responses, and high latency can lead to a poor user experience.\n\n2. Improved user experience: Low latency LLMs provide a more seamless and responsive user experience. Users are more likely to continue using a service that provides quick and accurate responses, leading to higher user engagement and satisfaction.\n\n3. Competitive advantage: In today's fast-paced digital world, businesses that can provide quick and accurate responses to customer inquiries have a competitive advantage. Low latency LLMs can help businesses respond to customer inquiries more quickly, potentially leading to increased sales and customer loyalty.\n\n4. Better decision-making: Low latency LLMs can provide real-time insights and recommendations, enabling businesses to make better decisions more quickly. This can be particularly important in industries such as finance, healthcare, and logistics, where quick decision-making can have a significant impact on business outcomes.\n\n5. Scalability: Low latency LLMs can handle a higher volume of requests, making them more scalable than high-latency models. This is particularly important for businesses that experience spikes in traffic or have a large user base.\n\nIn summary, low latency LLMs are essential for real-time applications, providing a better user experience, enabling quick decision-making, and improving scalability. As the demand for real-time NLP applications continues to grow, the importance of low latency LLMs will only become more critical."
      },
      "finish_reason": "stop",
      "logprobs": null
    }
  ],
  "usage": {
    "prompt_tokens": 24,
    "completion_tokens": 377,
    "total_tokens": 401,
    "prompt_time": 0.009,
    "completion_time": 0.774,
    "total_time": 0.783
  },
  "x_groq": {
    "id": "req_01htzpsmfmew5b4rbmbjy2kv74"
  }
}
```

### Groq community libraries
Groq encourages our developer community to build on our SDK. If you would like your library added, please fill out this form.

Please note that Groq does not verify the security of these projects. Use at your own risk.

### C#
- jgravelle.GroqAPILibrary by jgravelle

### Dart/Flutter
- TAGonSoft.groq-dart by TAGonSoft

### PHP
- lucianotonet.groq-php by lucianotonet

# Accounts

## API keys
API keys are required for accessing the APIs. You can manage your API keys here.
API Keys are bound to the organization, not the user.

## Rate Limits
Rate limits act as control measures to regulate how frequently a user or application can make requests within a given timeframe.

### Current rate limits for chat completions:
You can view the current rate limits for chat completions in your organization settings


The team is working on introducing paid tiers with stable and increased rate limits in the near future.

### Status code & rate limit headers
We set the following x-ratelimit headers to inform you on current rate limits applicable to the API key and associated organization.


The following headers are set (values are illustrative):

| Header | Value | Notes |
| --- | --- | --- |
| retry-after | 2 | In seconds |
| x-ratelimit-limit-requests |	14400 |	Always refers to Requests Per Day (RPD) |
| x-ratelimit-limit-tokens| 	18000| 	Always refers to Tokens Per Minute (TPM) |
| x-ratelimit-remaining-requests| 	14370	| Always refers to Requests Per Day (RPD) |
| x-ratelimit-remaining-tokens| 	17997	| Always refers to Tokens Per Minute (TPM) |
| x-ratelimit-reset-requests| 	2m59.56s	| Always refers to Requests Per Day (RPD) |
| x-ratelimit-reset-tokens| 	7.66s	| Always refers to Tokens Per Minute (TPM) |

When the rate limit is reached we return a 429 Too Many Requests HTTP status code.


Note, retry-after is only set if you hit the rate limit and status code 429 is returned. The other headers are always included.