<template>
  <div class="container">
    <h1 class="title">Máquina Expendedora</h1>

    <!-- Lista de bebidas -->
    <div class="drink-card" v-for="drink in drinks" :key="drink.name">
      <div class="drink-info">
        <h2>{{ drink.name }}</h2>
        <p>₡{{ drink.price }}</p>
        <p>Disponibles: {{ drink.quantity }}</p>
      </div>
      <div class="actions">
        <div class="counter">
          <button @click="decrease(drink)" :disabled="quantities[drink.name] <= 0">−</button>
          <span class="count">{{ quantities[drink.name] }}</span>
          <button @click="increase(drink)" :disabled="quantities[drink.name] >= drink.quantity">+</button>
        </div>
      </div>
    </div>

    <!-- Carrito -->
    <div class="cart">
      <h2>Carrito</h2>
      <div v-if="cartItems.length === 0">
        <p>Selecciona bebidas para ver el resumen aquí.</p>
      </div>
      <ul v-else>
        <li v-for="item in cartItems" :key="item.name">
          {{ item.name }} x {{ item.quantity }} = ₡{{ item.quantity * item.price }}
        </li>
      </ul>
    </div>

    <!-- Pago -->
    <div class="payment">
      <h2>Ingresar dinero</h2>
      <div class="coins">
        <div class="coin-input" v-for="denom in validCoins" :key="denom">
          <span>{{ denom }} colones</span>
          <div class="coin-counter">
            <button @click="updateCoin(denom, -1)" :disabled="insertedMoney[denom] === 0">−</button>
            <span>{{ insertedMoney[denom] }}</span>
            <button @click="updateCoin(denom, 1)">+</button>
          </div>
        </div>
      </div>

      <div class="summary">
        <p>Total a pagar: ₡{{ total }}</p>
        <p>Total ingresado: ₡{{ totalPaid }}</p>
      </div>

      <button
        class="pay-button"
        @click="payCart"
        :disabled="total === 0 || totalPaid < total"
      >
        🧾 Pagar todo
      </button>
    </div>
  </div>

  <!-- Popup del vuelto -->
  <div v-if="showModal" class="modal-overlay">
    <div class="modal-content">
      <h2>✅ Compra realizada</h2>
      <p>Su vuelto es de ₡{{ changeAmount }}</p>
      <ul>
        <li v-for="(qty, coin) in changeResult" :key="coin">
          {{ qty }} moneda(s) de {{ coin }} colones
        </li>
      </ul>
      <button @click="reloadPage" class="modal-button">Volver a comprar</button>
    </div>
  </div>
</template>

<script setup>
import '../assets/drinks.css'
import { ref, onMounted, computed } from 'vue'
import axios from 'axios'

// Bebidas y carrito
const drinks = ref([])
const quantities = ref({})
const cart = ref(Object.create(null))

const fetchDrinks = async () => {
  const res = await axios.get('http://localhost:5110/api/drinks')
  drinks.value = res.data
  res.data.forEach(drink => {
    quantities.value[drink.name] = 0
  })
}

const updateCart = (drink) => {
  cart.value[drink.name] = {
    name: drink.name,
    quantity: quantities.value[drink.name],
    price: drink.price
  }
}

const increase = (drink) => {
  const current = quantities.value[drink.name] || 0
  if (current < drink.quantity) {
    quantities.value[drink.name] = current + 1
    updateCart(drink)
  }
}

const decrease = (drink) => {
  const current = quantities.value[drink.name] || 0
  if (current > 0) {
    quantities.value[drink.name] = current - 1
    updateCart(drink)
  }
}

const cartItems = computed(() =>
  Object.values(cart.value).filter(item => item.quantity > 0)
)

const total = computed(() =>
  cartItems.value.reduce((sum, item) => sum + item.quantity * item.price, 0)
)

const validCoins = [1000, 500, 100, 50, 25]
const insertedMoney = ref({ 1000: 0, 500: 0, 100: 0, 50: 0, 25: 0 })

const totalPaid = computed(() =>
  Object.entries(insertedMoney.value).reduce(
    (sum, [coin, qty]) => sum + parseInt(coin) * qty,
    0
  )
)

const changeAmount = ref(0)
const changeResult = ref(null)
const showModal = ref(false)

const updateCoin = (denom, delta) => {
  const current = insertedMoney.value[denom]
  const updated = current + delta
  insertedMoney.value[denom] = updated < 0 ? 0 : updated
}


const payCart = async () => {
  if (cartItems.value.length === 0) {
    alert("No hay bebidas seleccionadas")
    return
  }

  if (totalPaid.value < total.value) {
    alert("Monto ingresado insuficiente")
    return
  }

  try {
    const res = await axios.post('http://localhost:5110/api/payments/pay', {
      amountPaid: totalPaid.value,
      totalToPay: total.value,
      items: cartItems.value
    })

    changeResult.value = res.data.changeBreakdown
    changeAmount.value = res.data.totalChange
    showModal.value = true

    await fetchDrinks()
    resetCart()
  } catch (err) {
    alert(err.response?.data || "Fallo al realizar la compra")
  }
}

const reloadPage = () => {
  window.location.reload()
}

const resetCart = () => {
  for (const key in cart.value) {
    cart.value[key].quantity = 0
    quantities.value[key] = 0
  }
  for (const coin of validCoins) {
    insertedMoney.value[coin] = 0
  }
}
onMounted(fetchDrinks)
</script>
