

window.addEventListener("load", async () => {
    const navSection = document.querySelector("#nav-section");
    const navHtml = `<nav class="navbar navbar-expand-lg navbar-dark bg-dark">
    <div class="container">
        <a class="navbar-brand" href="https://cstewart2010.github.io">
            Charles Stewart
        </a>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#nav" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div id="nav" class="collapse navbar-collapse">
            <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                <li class="nav-item">
                    <a class="nav-link" href="https://www.linkedin.com/in/charles-stewart-5a254182/" target="_blank" rel="noopener noreferrer">
                        Linkedin
                    </a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" href="https://github.com/cstewart2010" target="_blank" rel="noopener noreferrer">
                        Github
                    </a>
                </li>
                <li class="nav-item dropdown">
                    <a class="nav-link dropdown-toggle" href="#" id="navbarDropdown" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                        Projects
                    </a>
                    <ul class="dropdown-menu" aria-labelledby="navbarDropdown">
                        <li>
                            <a class="dropdown-item" href="https://pta-frontend.pages.dev/" target="_blank" rel="noopener noreferrer">
                                Pokemon Tabletop Adventures WebApp
                            </a>
                        </li>
                        <li>
                            <a class="dropdown-item" href="https://cstewart2010.github.io/verble">
                                Verble
                            </a>
                        </li>
                        <li>
                            <a class="dropdown-item" href="https://cstewart2010.github.io/taxEstimator">
                                Tax Estimator
                            </a>
                        </li>
                        <li>
                            <a class="dropdown-item" href="https://cstewart2010.github.io/shiny-calculator">
                                Pokemon Shiny Calculator
                            </a>
                        </li>
                    </ul>
                </li>
            </ul>
        </div>
    </div>
</nav>`;
    navSection.innerHTML = navHtml;
});