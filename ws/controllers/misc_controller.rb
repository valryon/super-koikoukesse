# Various tests

# Encryption tests
get '/test/e' do
  return Encryption.encrypt('{"player": "G1725278793", "coins": 2500}')
end

get '/test/d' do
  return Encryption.decrypt('hPZDj7epdyCMKwyeevCIfb+uMp0//e4TfjSuRPWLQR864bQt+MgoVFkLhbMIuHk8')
end
