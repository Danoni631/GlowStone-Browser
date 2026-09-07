function getLocation() {
      const t = translations[currentLang];
      if (navigator.geolocation) {
        document.getElementById("content").innerText = t.loading;
        navigator.geolocation.getCurrentPosition(fetchWeather, showError);
      } else {
        document.getElementById("content").innerText = t.noGeo;
      }
    }

    async function fetchWeather(position) {
      const lat = position.coords.latitude;
      const lon = position.coords.longitude;

      try {
        const response = await fetch(`https://api.open-meteo.com/v1/forecast?latitude=${lat}&longitude=${lon}&current=temperature_2m,relative_humidity_2m,apparent_temperature,weather_code,wind_speed_10m`);
        const data = await response.json();
        currentData = data.current;
        renderWeather();
      } catch (err) {
        document.getElementById("content").innerText = translations[currentLang].error;
      }
    }

    function renderWeather() {
      if (!currentData) return;
      const t = translations[currentLang];
      const condition = t.weather[currentData.weather_code] || "—";
      
      document.getElementById("content").innerHTML = `
        <div class="city">${t.location}</div>
        <div class="temp">${Math.round(currentData.temperature_2m)}°C</div>
        <div class="condition">${condition}</div>
        <div class="grid">
          <div class="item">
            <span class="label">${t.feelsLike}</span>
            <span class="value">${Math.round(currentData.apparent_temperature)}°C</span>
          </div>
          <div class="item">
            <span class="label">${t.humidity}</span>
            <span class="value">${currentData.relative_humidity_2m}%</span>
          </div>
          <div class="item" style="grid-column: span 2;">
            <span class="label">${t.wind}</span>
            <span class="value">${currentData.wind_speed_10m} km/h</span>
          </div>
        </div>
      `;
    }

    function changeLanguage() {
      currentLang = document.getElementById("langSelect").value;
      if (currentData) {
        renderWeather();
      } else {
        getLocation();
      }
    }

    function showError() {
      document.getElementById("content").innerText = translations[currentLang].denied;
    }

    getLocation();