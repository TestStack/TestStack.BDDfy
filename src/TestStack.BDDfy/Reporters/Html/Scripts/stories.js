var STORIES_DATA = {
  "Summary": {
    "Namespaces": 1,
    "Scenarios": 2,
    "Stories": 0,
    "Passed": 2,
    "Failed": 0,
    "Inconclusive": 0,
    "NotImplemented": 0
  },
  "RunDate": "2026-06-05T16:22:01.9369023+01:00",
  "Stories": [
    {
      "Namespace": "TestStack.BDDfy.Samples",
      "Result": "Passed",
      "Scenarios": [
        {
          "Id": "scenario-1",
          "Title": "Run examples with fluent api",
          "Tags": [],
          "Example": {
            "Headers": [
              "Start",
              "Eat",
              "Left"
            ],
            "Values": [
              {
                "Header": "Start",
                "Row": 1,
                "ValueHasBeenUsed": true
              },
              {
                "Header": "Eat",
                "Row": 1,
                "ValueHasBeenUsed": true
              },
              {
                "Header": "Left",
                "Row": 1,
                "ValueHasBeenUsed": true
              }
            ]
          },
          "Duration": "00:00:00.0324101",
          "Steps": [
            {
              "Id": "step-1-1",
              "Asserts": false,
              "ShouldReport": true,
              "Title": "Given there are \u003Cstart\u003E cucumbers",
              "ExecutionOrder": "SetupState",
              "Result": "Passed",
              "Exception": null,
              "Duration": "00:00:00.0002658"
            },
            {
              "Id": "step-1-2",
              "Asserts": false,
              "ShouldReport": true,
              "Title": "And I steal two more",
              "ExecutionOrder": "ConsecutiveSetupState",
              "Result": "Passed",
              "Exception": null,
              "Duration": "00:00:00.0001098"
            },
            {
              "Id": "step-1-3",
              "Asserts": false,
              "ShouldReport": true,
              "Title": "When I eat \u003Ceat\u003E cucumbers",
              "ExecutionOrder": "Transition",
              "Result": "Passed",
              "Exception": null,
              "Duration": "00:00:00.0000353"
            },
            {
              "Id": "step-1-4",
              "Asserts": true,
              "ShouldReport": true,
              "Title": "Then I should have \u003Cleft\u003E cucumbers",
              "ExecutionOrder": "Assertion",
              "Result": "Passed",
              "Exception": null,
              "Duration": "00:00:00.0319992"
            }
          ],
          "Result": "Passed"
        },
        {
          "Id": "scenario-1",
          "Title": "Run examples with fluent api",
          "Tags": [],
          "Example": {
            "Headers": [
              "Start",
              "Eat",
              "Left"
            ],
            "Values": [
              {
                "Header": "Start",
                "Row": 2,
                "ValueHasBeenUsed": true
              },
              {
                "Header": "Eat",
                "Row": 2,
                "ValueHasBeenUsed": true
              },
              {
                "Header": "Left",
                "Row": 2,
                "ValueHasBeenUsed": true
              }
            ]
          },
          "Duration": "00:00:00.0000255",
          "Steps": [
            {
              "Id": "step-1-1",
              "Asserts": false,
              "ShouldReport": true,
              "Title": "Given there are \u003Cstart\u003E cucumbers",
              "ExecutionOrder": "SetupState",
              "Result": "Passed",
              "Exception": null,
              "Duration": "00:00:00.0000013"
            },
            {
              "Id": "step-1-2",
              "Asserts": false,
              "ShouldReport": true,
              "Title": "AndIStealTwoMore",
              "ExecutionOrder": "ConsecutiveSetupState",
              "Result": "Passed",
              "Exception": null,
              "Duration": "00:00:00.0000006"
            },
            {
              "Id": "step-1-3",
              "Asserts": false,
              "ShouldReport": true,
              "Title": "WhenIEat__eat__Cucumbers",
              "ExecutionOrder": "Transition",
              "Result": "Passed",
              "Exception": null,
              "Duration": "00:00:00.0000002"
            },
            {
              "Id": "step-1-4",
              "Asserts": true,
              "ShouldReport": true,
              "Title": "ThenIShouldHave__left__Cucumbers",
              "ExecutionOrder": "Assertion",
              "Result": "Passed",
              "Exception": null,
              "Duration": "00:00:00.0000234"
            }
          ],
          "Result": "Passed"
        }
      ],
      "Metadata": null
    }
  ]
}