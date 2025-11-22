const cost = 590;

    function storeBooking() {
      const name = document.getElementById('name').value.trim();
      const seat = document.getElementById('seat').value;
      const movieTitle = document.getElementById('movie-title').textContent;

      if (!name) {
        alert("Please enter your name.");
        return;
      }

      const bookingData = {
        name: name,
        seat: seat,
        movieTitle: movieTitle,
        cost: cost
      };

      // Send to backend
      fetch("http://localhost:5501/api/storeBooking", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(bookingData)
      })
      .then(response => response.text())
      .then(result => alert(result))
      .catch(error => console.error("Error:", error));
    }