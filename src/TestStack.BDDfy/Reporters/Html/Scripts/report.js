// ─── Setup htm for JSX-like templates without Babel ─────────────────────────
var html = htm.bind(React.createElement);
var useState = React.useState;
var useMemo = React.useMemo;

// Data is loaded from stories.js (sets window.STORIES_DATA via <script src="stories.js">)

// ─── Utility Functions ───────────────────────────────────────────────────────

function formatDuration(duration) {
    if (!duration) return '';
    var parts = duration.split(':');
    var seconds = parseFloat(parts[2]);
    if (seconds < 0.001) return '<1ms';
    if (seconds < 1) return (seconds * 1000).toFixed(1) + 'ms';
    return seconds.toFixed(2) + 's';
}

function formatDate(dateStr) {
    var date = new Date(dateStr);
    return date.toLocaleDateString('en-GB', {
        weekday: 'long',
        year: 'numeric',
        month: 'long',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    });
}

function getResultClass(result) {
    switch (result) {
        case 'Passed': return 'is-passed';
        case 'Failed': return 'is-failed';
        case 'Inconclusive': return 'is-inconclusive';
        case 'NotImplemented': return 'is-not-implemented';
        default: return '';
    }
}

function getResultIcon(result) {
    switch (result) {
        case 'Passed': return 'fa-circle-check';
        case 'Failed': return 'fa-circle-xmark';
        case 'Inconclusive': return 'fa-circle-question';
        case 'NotImplemented': return 'fa-circle-minus';
        default: return 'fa-circle';
    }
}

function getStepKeyword(title, executionOrder) {
    if (title.startsWith('Given')) return 'Given';
    if (title.startsWith('When')) return 'When';
    if (title.startsWith('Then')) return 'Then';
    if (title.startsWith('And')) return 'And';
    if (executionOrder.includes('SetupState') && !executionOrder.includes('Consecutive')) return 'Given';
    if (executionOrder.includes('Transition')) return 'When';
    if (executionOrder.includes('Assertion') && !executionOrder.includes('Consecutive')) return 'Then';
    if (executionOrder.includes('Consecutive')) return 'And';
    return '';
}

function getStepText(title, keyword) {
    if (title.startsWith(keyword)) {
        return title.substring(keyword.length).trim();
    }
    return title;
}

// ─── Components ──────────────────────────────────────────────────────────────

function ResultBadge(props) {
    var result = props.result;
    return html`
        <span className=${'result-badge ' + getResultClass(result)}>
            <i className=${'fas ' + getResultIcon(result)}></i>
            ${' ' + result}
        </span>
    `;
}

function SummaryCards(props) {
    var summary = props.summary;
    var cards = [
        { label: 'Scenarios', value: summary.Scenarios, className: 'is-total', icon: 'fa-list-check' },
        { label: 'Passed', value: summary.Passed, className: 'is-passed', icon: 'fa-circle-check' },
        { label: 'Failed', value: summary.Failed, className: 'is-failed', icon: 'fa-circle-xmark' },
        { label: 'Inconclusive', value: summary.Inconclusive, className: 'is-inconclusive', icon: 'fa-circle-question' },
        { label: 'Not Implemented', value: summary.NotImplemented, className: 'is-not-implemented', icon: 'fa-circle-minus' }
    ];

    return html`
        <div className="summary-cards">
            <div className="columns is-multiline is-mobile">
                ${cards.map(function(card, idx) {
                    return html`
                        <div className="column is-half-mobile is-one-fifth-desktop" key=${idx}>
                            <div className=${'summary-card ' + card.className}>
                                <div className="stat-number">
                                    <i className=${'fas ' + card.icon} style=${{ fontSize: '0.6em', marginRight: '0.3em' }}></i>
                                    ${card.value}
                                </div>
                                <div className="stat-label">${card.label}</div>
                            </div>
                        </div>
                    `;
                })}
            </div>
        </div>
    `;
}

function ProgressBar(props) {
    var summary = props.summary;
    var total = summary.Scenarios;
    var passedPct = total > 0 ? (summary.Passed / total * 100) : 0;

    return html`
        <div className="progress-overview">
            <div className="is-flex is-justify-content-space-between mb-1">
                <span className="is-size-7 has-text-grey">Pass Rate</span>
                <span className="is-size-7 has-text-weight-bold">${passedPct.toFixed(1)}%</span>
            </div>
            <progress className="progress is-success is-small" value=${summary.Passed} max=${total}>
                ${passedPct}%
            </progress>
        </div>
    `;
}

function StepItem(props) {
    var step = props.step;
    var keyword = getStepKeyword(step.Title, step.ExecutionOrder);
    var text = getStepText(step.Title, keyword);
    var keywordClass = keyword.toLowerCase();
    var iconColor = step.Result === 'Passed' ? 'has-text-success' : 'has-text-danger';

    return html`
        <div className="step-item">
            <span className="step-icon">
                <i className=${'fas ' + getResultIcon(step.Result) + ' ' + iconColor}></i>
            </span>
            <span className=${'step-keyword is-' + keywordClass}>${keyword}</span>
            <span className="step-text">${text}</span>
            <span className="step-duration">${formatDuration(step.Duration)}</span>
        </div>
    `;
}

function ExceptionBlock(props) {
    if (!props.exception) return null;
    return html`
        <div className="exception-block">
            <i className="fas fa-bug mr-2"></i>
            ${props.exception}
        </div>
    `;
}

function ExampleTable(props) {
    var example = props.example;
    if (!example) return null;
    return html`
        <div className="example-table">
            <table className="table is-bordered is-narrow is-fullwidth">
                <thead>
                    <tr>
                        ${example.Headers.map(function(h, i) {
                            return html`<th key=${i}>${h}</th>`;
                        })}
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        ${example.Values.map(function(v, i) {
                            return html`<td key=${i}>Row ${v.Row}</td>`;
                        })}
                    </tr>
                </tbody>
            </table>
        </div>
    `;
}

function ScenarioItem(props) {
    var scenario = props.scenario;
    var openState = useState(scenario.Result === 'Failed');
    var isOpen = openState[0];
    var setIsOpen = openState[1];

    var exampleRow = scenario.Example && scenario.Example.Values[0] ? scenario.Example.Values[0].Row : null;

    return html`
        <div className="scenario-item">
            <div className="scenario-header" onClick=${function() { setIsOpen(!isOpen); }}>
                <i className=${'fas fa-chevron-right chevron ' + (isOpen ? 'is-open' : '')}></i>
                <span className="scenario-title">${scenario.Title}</span>
                ${scenario.Example && html`
                    <span className="tag is-light is-info is-small mr-2">
                        <i className="fas fa-table mr-1"></i>
                        Row ${exampleRow}
                    </span>
                `}
                <span className="scenario-duration">${formatDuration(scenario.Duration)}</span>
                <${ResultBadge} result=${scenario.Result} />
            </div>
            <div className=${'scenario-steps ' + (isOpen ? 'is-open' : '')}>
                ${scenario.Example && html`<${ExampleTable} example=${scenario.Example} />`}
                ${scenario.Steps.filter(function(s) { return s.ShouldReport; }).map(function(step) {
                    return html`
                        <div key=${step.Id}>
                            <${StepItem} step=${step} />
                            ${step.Exception && html`<${ExceptionBlock} exception=${step.Exception} />`}
                        </div>
                    `;
                })}
            </div>
        </div>
    `;
}

function StoryMetadata(props) {
    var metadata = props.metadata;
    if (!metadata) return null;

    return html`
        <div className="story-metadata">
            <div className="is-flex is-align-items-center" style=${{ gap: '1rem' }}>
                ${metadata.ImageUri && html`
                    <img src=${metadata.ImageUri} alt="" className="story-image" loading="lazy" />
                `}
                <div>
                    <div className="story-title">
                        ${metadata.TitlePrefix}${metadata.Title}
                    </div>
                    <div className="narrative">
                        ${metadata.Narrative1 && html`<div>${metadata.Narrative1}</div>`}
                        ${metadata.Narrative2 && html`<div>${metadata.Narrative2}</div>`}
                        ${metadata.Narrative3 && html`<div>${metadata.Narrative3}</div>`}
                    </div>
                    ${metadata.StoryUri && html`
                        <div className="story-links">
                            <a href=${metadata.StoryUri} target="_blank" rel="noopener noreferrer">
                                <i className="fas fa-external-link-alt mr-1"></i>Story Link
                            </a>
                        </div>
                    `}
                </div>
            </div>
        </div>
    `;
}

function NamespaceGroup(props) {
    var namespace = props.namespace;
    var stories = props.stories;
    var openState = useState(true);
    var isOpen = openState[0];
    var setIsOpen = openState[1];

    var totalScenarios = stories.reduce(function(sum, s) { return sum + s.Scenarios.length; }, 0);
    var hasFailure = stories.some(function(s) { return s.Result === 'Failed'; });

    return html`
        <div className="namespace-group">
            <div className="namespace-header" onClick=${function() { setIsOpen(!isOpen); }}>
                <div className="namespace-title">
                    <i className=${'fas fa-chevron-right chevron ' + (isOpen ? 'is-open' : '')}></i>
                    <i className=${'fas fa-folder' + (isOpen ? '-open' : '') + ' has-text-warning'}></i>
                    <span>${namespace}</span>
                    <span className="scenario-count">
                        ${totalScenarios} scenario${totalScenarios !== 1 ? 's' : ''}
                    </span>
                </div>
                <${ResultBadge} result=${hasFailure ? 'Failed' : 'Passed'} />
            </div>
            <div className=${'namespace-content ' + (isOpen ? 'is-open' : '')}>
                ${stories.map(function(story, idx) {
                    return html`
                        <div key=${idx}>
                            <${StoryMetadata} metadata=${story.Metadata} />
                            ${story.Scenarios.map(function(scenario) {
                                var key = scenario.Id + '-' + (scenario.Example && scenario.Example.Values[0] ? scenario.Example.Values[0].Row : '');
                                return html`<${ScenarioItem} key=${key} scenario=${scenario} />`;
                            })}
                        </div>
                    `;
                })}
            </div>
        </div>
    `;
}

function FilterBar(props) {
    var filter = props.filter;
    var onFilterChange = props.onFilterChange;
    var searchTerm = props.searchTerm;
    var onSearchChange = props.onSearchChange;

    var filters = [
        { key: 'all', label: 'All', icon: 'fa-list' },
        { key: 'passed', label: 'Passed', icon: 'fa-circle-check' },
        { key: 'failed', label: 'Failed', icon: 'fa-circle-xmark' }
    ];

    return html`
        <div className="is-flex is-align-items-center is-flex-wrap-wrap mb-4" style=${{ gap: '0.75rem' }}>
            <div className="buttons has-addons mb-0">
                ${filters.map(function(f) {
                    return html`
                        <button
                            key=${f.key}
                            className=${'button is-small ' + (filter === f.key ? 'is-dark is-selected' : '')}
                            onClick=${function() { onFilterChange(f.key); }}
                        >
                            <span className="icon is-small"><i className=${'fas ' + f.icon}></i></span>
                            <span>${f.label}</span>
                        </button>
                    `;
                })}
            </div>
            <div className="control has-icons-left" style=${{ flex: 1, minWidth: '200px' }}>
                <input
                    className="input is-small"
                    type="search"
                    placeholder="Search scenarios..."
                    value=${searchTerm}
                    onInput=${function(e) { onSearchChange(e.target.value); }}
                />
                <span className="icon is-small is-left">
                    <i className="fas fa-search"></i>
                </span>
            </div>
        </div>
    `;
}

function App() {
    var data = STORIES_DATA;
    var filterState = useState('all');
    var filter = filterState[0];
    var setFilter = filterState[1];
    var searchState = useState('');
    var searchTerm = searchState[0];
    var setSearchTerm = searchState[1];

    // Group stories by namespace
    var groupedByNamespace = useMemo(function() {
        var groups = {};
        data.Stories.forEach(function(story) {
            var ns = story.Namespace;
            if (!groups[ns]) groups[ns] = [];
            groups[ns].push(story);
        });
        return groups;
    }, [data]);

    // Apply filters
    var filteredGroups = useMemo(function() {
        var result = {};
        Object.entries(groupedByNamespace).forEach(function(entry) {
            var ns = entry[0];
            var stories = entry[1];
            var filteredStories = stories.map(function(story) {
                var scenarios = story.Scenarios;

                if (filter !== 'all') {
                    scenarios = scenarios.filter(function(s) {
                        return s.Result.toLowerCase() === filter;
                    });
                }

                if (searchTerm) {
                    var term = searchTerm.toLowerCase();
                    scenarios = scenarios.filter(function(s) {
                        return s.Title.toLowerCase().includes(term) ||
                            s.Steps.some(function(step) { return step.Title.toLowerCase().includes(term); });
                    });
                }

                return Object.assign({}, story, { Scenarios: scenarios });
            }).filter(function(story) { return story.Scenarios.length > 0; });

            if (filteredStories.length > 0) {
                result[ns] = filteredStories;
            }
        });
        return result;
    }, [groupedByNamespace, filter, searchTerm]);

    return html`
        <div>
            <header className="report-header">
                <div className="container">
                    <div className="is-flex is-align-items-center is-justify-content-space-between is-flex-wrap-wrap">
                        <div>
                            <h1 className="title is-3 mb-1">
                                <i className="fas fa-vial mr-2"></i>
                                BDDfy Test Report
                            </h1>
                            <p className="subtitle is-6">
                                <i className="fas fa-calendar-alt mr-1"></i>
                                ${formatDate(data.RunDate)}
                            </p>
                        </div>
                        <div className="has-text-right">
                            <span className="tag is-medium is-light">
                                <i className="fas fa-layer-group mr-1"></i>
                                ${data.Summary.Namespaces} Namespaces
                            </span>
                            <span className="tag is-medium is-light ml-2">
                                <i className="fas fa-book mr-1"></i>
                                ${data.Summary.Stories} Stories
                            </span>
                        </div>
                    </div>
                </div>
            </header>

            <main className="container">
                <${SummaryCards} summary=${data.Summary} />
                <${ProgressBar} summary=${data.Summary} />
                <${FilterBar}
                    filter=${filter}
                    onFilterChange=${setFilter}
                    searchTerm=${searchTerm}
                    onSearchChange=${setSearchTerm}
                />

                <section>
                    ${Object.entries(filteredGroups).map(function(entry) {
                        var namespace = entry[0];
                        var stories = entry[1];
                        return html`
                            <${NamespaceGroup}
                                key=${namespace}
                                namespace=${namespace}
                                stories=${stories}
                            />
                        `;
                    })}
                    ${Object.keys(filteredGroups).length === 0 && html`
                        <div className="notification is-light has-text-centered">
                            <i className="fas fa-search mr-2"></i>
                            No scenarios match the current filter.
                        </div>
                    `}
                </section>
            </main>

            <footer className="report-footer">
                <p>
                    Generated by <strong>TestStack.BDDfy</strong> •${' '}
                    <i className="fas fa-clock ml-1 mr-1"></i>
                    ${formatDate(data.RunDate)}
                </p>
            </footer>
        </div>
    `;
}

// ─── Render ──────────────────────────────────────────────────────────────────

var root = ReactDOM.createRoot(document.getElementById('root'));
root.render(html`<${App} />`);
